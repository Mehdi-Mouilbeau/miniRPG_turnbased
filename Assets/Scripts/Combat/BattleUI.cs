using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour, IBattleUI
{
    [Header("Visual References")]
    public Transform playerVisual;
    public Transform enemyVisual;

    [Header("UI Elements")]
    public Slider playerHP;
    public Slider playerMana;
    public Slider enemyHP;
    public Slider enemyMana;
    public GameObject frozenIcon;

    public void UpdateCharacterStats(Character character, bool isPlayer)
    {
        if (isPlayer)
        {
            if (playerHP != null) playerHP.value = (float)character.HP / character.MaxHP;
            if (playerMana != null) playerMana.value = (float)character.Mana / character.MaxMana;
        }
        else
        {
            if (enemyHP != null) enemyHP.value = (float)character.HP / character.MaxHP;
            if (enemyMana != null) enemyMana.value = (float)character.Mana / character.MaxMana;
        }
    }

    public void UpdateFrozenStatus(bool isFrozen)
    {
        if (frozenIcon != null)
        {
            frozenIcon.SetActive(isFrozen);
        }
    }

    public void PlayAttackAnimation(bool attackerIsPlayer)
    {
        Transform attacker = attackerIsPlayer ? playerVisual : enemyVisual;
        Transform defender = attackerIsPlayer ? enemyVisual : playerVisual;

        if (attacker != null && defender != null)
        {
            StartCoroutine(AttackAnimationCoroutine(attacker, defender));
        }
    }

    private IEnumerator AttackAnimationCoroutine(Transform attacker, Transform defender)
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
}