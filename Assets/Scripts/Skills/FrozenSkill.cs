using UnityEngine;

public class FrozenSkill : Skill
{
    public bool freezeSuccess;
    public FrozenSkill()
    {
        Name = "Frozen";
        ManaCost = 15;
        Cooldown = 5;
        TargetType = TargetType.Enemy;
    }

    protected override void OnUse(Character caster, Character target)
    {
        float chance = 0.5f;
        freezeSuccess = Random.value < chance;

        if (freezeSuccess)
        {
            target.IsFrozen = true;
            target.FrozenTurns = 2; 
            Debug.Log(target.Name + " is frozen by " + caster.Name + "'s Frozen skill!");
        }
        else
        {
            Debug.Log(caster.Name + "'s Frozen skill failed to freeze " + target.Name + ".");
        }
    }

}