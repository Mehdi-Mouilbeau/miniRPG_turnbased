using System.Collections;
using UnityEngine;
using TMPro;
public class BattleManager : MonoBehaviour, IBattleLogger
{
    [Header("UI")]
    public BattleUI battleUI;
    public TextMeshProUGUI battleLogText;

    [Header("Settings")]
    public float turnDelay = 0.5f;

    private BattleLogic battleLogic;

    void Start()
    {
        if (battleLogText != null)
            battleLogText.text = "";

        Character player = new Mage();
        Character enemy = new Warrior();

        battleLogic = new BattleLogic(player, enemy, this, new DefaultAIStrategy());
        battleLogic.LogBattleStart();

        UpdateUI();
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(turnDelay);

        while (!battleLogic.IsGameOver)
        {
            BattleTurnResult result = battleLogic.ProcessTurn();

            if (result.WasBasicAttack)
            {
                battleUI.PlayAttackAnimation(result.ActorIsPlayer);
            }

            UpdateUI();

            if (result.GameEnded)
            {
                EndBattle();
                break;
            }

            yield return new WaitForSeconds(turnDelay);
        }
    }

    private void UpdateUI()
    {
        battleUI.UpdateCharacterStats(battleLogic.Player, true);
        battleUI.UpdateCharacterStats(battleLogic.Enemy, false);
        battleUI.UpdateFrozenStatus(battleLogic.Enemy.IsFrozen);
    }

    public void Log(string message)
    {
        Debug.Log(message);
        if (battleLogText != null)
            battleLogText.text += message + "\n";
    }

    private void EndBattle()
    {
        Log("\n=== FIN DU COMBAT ===");
        string winner = battleLogic.GetWinner();
        
        if (winner == battleLogic.Player.Name)
            Log($"{winner} a gagné !");
        else if (winner == battleLogic.Enemy.Name)
            Log($"{winner} a gagné !");
        else
            Log(winner);
    }
}