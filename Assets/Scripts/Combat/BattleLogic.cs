public class BattleLogic
{
    private Character player;
    private Character enemy;
    private Character currentTurnCharacter;
    private Character otherCharacter;
    private int turnNumber = 0;

    private IBattleLogger logger;
    private IAIStrategy aiStrategy;

    public Character Player => player;
    public Character Enemy => enemy;
    public bool IsGameOver => !player.IsAlive() || !enemy.IsAlive();
    public int TurnNumber => turnNumber;

    public BattleLogic(Character playerChar, Character enemyChar, IBattleLogger logger, IAIStrategy aiStrategy = null)
    {
        this.player = playerChar;
        this.enemy = enemyChar;
        this.logger = logger;
        this.aiStrategy = aiStrategy ?? new DefaultAIStrategy();

        DetermineInitiative();
    }

    private void DetermineInitiative()
    {
        currentTurnCharacter = (player.Attack >= enemy.Attack) ? player : enemy;
        otherCharacter = (currentTurnCharacter == player) ? enemy : player;
    }

    public void LogBattleStart()
    {
        logger.Log("=== DÉBUT DU COMBAT ===");
        logger.Log($"{player.Name} vs {enemy.Name}");
        LogCharacterStats(player);
        LogCharacterStats(enemy);
    }

    public BattleTurnResult ProcessTurn()
    {
        if (IsGameOver)
        {
            return new BattleTurnResult { GameEnded = true };
        }

        turnNumber++;
        logger.Log($"\n--- Tour {turnNumber} : {currentTurnCharacter.Name} ---");

        ApplyTurnEffects();
        ReduceCooldowns();

        BattleTurnResult result = new BattleTurnResult
        {
            TurnNumber = turnNumber,
            ActingCharacter = currentTurnCharacter,
            TargetCharacter = otherCharacter,
            ActorIsPlayer = (currentTurnCharacter == player)
        };

        if (currentTurnCharacter.IsFrozen)
        {
            HandleFrozenTurn(result);
        }
        else
        {
            ExecuteAction(result);
        }

        LogCharacterStats(player);
        LogCharacterStats(enemy);

        SwapTurns();

        result.GameEnded = IsGameOver;
        return result;
    }

    private void ApplyTurnEffects()
    {
        player.TickEffects();
        enemy.TickEffects();
    }

    private void ReduceCooldowns()
    {
        ReduceCharacterCooldowns(player);
        ReduceCharacterCooldowns(enemy);
    }

    private void ReduceCharacterCooldowns(Character character)
    {
        if (character.Skills == null) return;

        foreach (Skill skill in character.Skills)
        {
            skill.ReduceCooldown();
        }
    }

    private void HandleFrozenTurn(BattleTurnResult result)
    {
        logger.Log($"{currentTurnCharacter.Name} est gelé et ne peut pas agir ce tour !");
        currentTurnCharacter.IsFrozen = false;
        result.WasFrozen = true;
    }

    private void ExecuteAction(BattleTurnResult result)
    {
        Skill chosenSkill = aiStrategy.ChooseAction(currentTurnCharacter, otherCharacter);

        if (chosenSkill != null)
        {
            Character target = (chosenSkill.TargetType == TargetType.Self) ? currentTurnCharacter : otherCharacter;
            chosenSkill.Use(currentTurnCharacter, target);
            result.SkillUsed = chosenSkill;
        }
        else
        {
            ExecuteBasicAttack(currentTurnCharacter, otherCharacter);
            result.WasBasicAttack = true;
        }
    }

    private void ExecuteBasicAttack(Character attacker, Character defender)
    {
        int damage = attacker.Attack;
        logger.Log($"{attacker.Name} attaque {defender.Name} !");
        defender.TakeDamage(damage);
    }

    private void SwapTurns()
    {
        Character temp = currentTurnCharacter;
        currentTurnCharacter = otherCharacter;
        otherCharacter = temp;
    }

    private void LogCharacterStats(Character character)
    {
        logger.Log($"{character.Name} HP:{character.HP}/{character.MaxHP} Mana:{character.Mana}/{character.MaxMana}");
    }

    public string GetWinner()
    {
        if (player.IsAlive() && !enemy.IsAlive()) return player.Name;
        if (!player.IsAlive() && enemy.IsAlive()) return enemy.Name;
        return "Match nul";
    }
}