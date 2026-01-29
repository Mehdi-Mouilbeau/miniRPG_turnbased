using UnityEngine;

public interface IBattleUI
{
    void UpdateCharacterStats(Character character, bool isPlayer);
    void UpdateFrozenStatus(bool isFrozen);
    void PlayAttackAnimation(bool attackerIsPlayer);
}