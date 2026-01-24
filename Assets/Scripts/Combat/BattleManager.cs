using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private Character player;
    private Character enemy;

    private Character currentTurnCharacter;
    private Character otherCharacter;

    public Transform playerVisual;
    public Transform enemyVisual;

    public Slider playerHP;
    public Slider playerMana;
    public Slider enemyHP;
    public Slider enemyMana;

    public GameObject frozenIcon;


    private int turnNumber = 0;

    public TextMeshProUGUI battleLogText;

    void Start()
    {
        if (battleLogText != null)
            battleLogText.text = "";

        // Initialize characters
        player = new Mage();
        enemy = new Warrior();

        // Initiative simple
        currentTurnCharacter = (player.Attack >= enemy.Attack) ? player : enemy;
        otherCharacter = (currentTurnCharacter == player) ? enemy : player;

        Log("=== DÉBUT DU COMBAT ===");
        Log($"{player.Name} vs {enemy.Name}");
        Log($"{player.Name} HP:{player.HP}/{player.MaxHP} Mana:{player.Mana}/{player.MaxMana}");
        Log($"{enemy.Name} HP:{enemy.HP}/{enemy.MaxHP} Mana:{enemy.Mana}/{enemy.MaxMana}");

        UpdateUI();

        NextTurn();
    }

    private void UpdateUI()
    {
        if (playerHP != null) playerHP.value = (float)player.HP / player.MaxHP;
        if (playerMana != null) playerMana.value = (float)player.Mana / player.MaxMana;

        if (enemyHP != null) enemyHP.value = (float)enemy.HP / enemy.MaxHP;
        if (enemyMana != null) enemyMana.value = (float)enemy.Mana / enemy.MaxMana;

        if (frozenIcon != null) frozenIcon.SetActive(enemy.IsFrozen);
    }


    private void Log(string msg)
    {
        Debug.Log(msg);
        if (battleLogText != null)
            battleLogText.text += msg + "\n";
    }

    private IEnumerator AttackAnim(Transform attacker, Transform defender)
    {
        Vector3 start = attacker.position;
        Vector3 toward = (start + defender.position) * 0.5f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            attacker.position = Vector3.Lerp(start, toward, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            attacker.position = Vector3.Lerp(toward, start, t);
            yield return null;
        }
    }



    private void NextTurn()
    {
        // Fin
        if (!player.IsAlive() || !enemy.IsAlive())
        {
            EndBattle();
            return;
        }

        turnNumber++;
        Log($"\n--- Tour {turnNumber} : {currentTurnCharacter.Name} ---");

        player.TickEffects();
        enemy.TickEffects();
        TickAllCooldowns(player);
        TickAllCooldowns(enemy);

        // if frozen, skip turn
        if (currentTurnCharacter.IsFrozen)
        {
            Log($"{currentTurnCharacter.Name} est gelé et ne peut pas agir ce tour !");
            currentTurnCharacter.IsFrozen = false;
            UpdateUI();
            SwapTurns();
            Invoke(nameof(NextTurn), 0.5f);
            return;
        }

        DoAutoAction(currentTurnCharacter, otherCharacter);

        UpdateUI();

        Log($"{player.Name} HP:{player.HP}/{player.MaxHP} Mana:{player.Mana}/{player.MaxMana}");
        Log($"{enemy.Name} HP:{enemy.HP}/{enemy.MaxHP} Mana:{enemy.Mana}/{enemy.MaxMana}");

        SwapTurns();
        Invoke(nameof(NextTurn), 0.5f);
    }

    private void DoAutoAction(Character actor, Character target)
    {
        Log(actor.Name + " skills: " + string.Join(", ", actor.Skills.ConvertAll(s => s.Name)));

        // 1) Guard if no defense buff
        Skill guard = FindSkillByName(actor, "Garde");
        if (guard != null && guard.CanUse(actor) && actor.DefenseBuffTurns == 0)
        {
            guard.Use(actor, actor);
            return;
        }

        // 2) Heal if HP < 50%
        Skill heal = FindSkillByName(actor, "Soin");
        if (heal != null && heal.CanUse(actor) && actor.HP < actor.MaxHP / 2)
        {
            heal.Use(actor, actor);
            return;
        }

        // 3) Offensive : 
        Skill offensive = ChooseBestEnemySkill(actor);
        if (offensive != null)
        {
            offensive.Use(actor, target);
            return;
        }

        // 4) else basic attack
        BasicAttack(actor, target);
    }

    // Rules : Skill with highest Cooldown first
    private Skill ChooseBestEnemySkill(Character actor)
    {
        if (actor.Skills == null) return null;

        Skill best = null;

        foreach (Skill s in actor.Skills)
        {
            if (s.TargetType != TargetType.Enemy) continue;
            if (!s.CanUse(actor)) continue;

            if (best == null ||
                s.Cooldown > best.Cooldown ||
                (s.Cooldown == best.Cooldown && s.ManaCost > best.ManaCost))
            {
                best = s;
            }
        }

        return best;
    }

    private void BasicAttack(Character attacker, Character defender)
    {
        int damage = attacker.Attack;
        Log($"{attacker.Name} attaque {defender.Name} !");
        Transform atk = (attacker == player) ? playerVisual : enemyVisual;
        Transform def = (defender == player) ? playerVisual : enemyVisual;
        StartCoroutine(AttackAnim(atk, def));

        defender.TakeDamage(damage);
    }

    private void TickAllCooldowns(Character c)
    {
        if (c.Skills == null) return;

        foreach (Skill s in c.Skills)
        {
            s.ReduceCooldown();
        }
    }

    private Skill FindSkillByName(Character c, string name)
    {
        if (c.Skills == null) return null;

        foreach (Skill s in c.Skills)
        {
            if (s.Name == name)
                return s;
        }
        return null;
    }

    private void SwapTurns()
    {
        Character tmp = currentTurnCharacter;
        currentTurnCharacter = otherCharacter;
        otherCharacter = tmp;
    }

    private void EndBattle()
    {
        Log("\n=== FIN DU COMBAT ===");
        if (player.IsAlive() && !enemy.IsAlive()) Log($"{player.Name} a gagné !");
        else if (!player.IsAlive() && enemy.IsAlive()) Log($"{enemy.Name} a gagné !");
        else Log("Match nul ?");
    }
}
