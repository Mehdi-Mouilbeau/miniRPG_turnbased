using UnityEngine;

public class ChargedSkill : Skill
{

    public int damage;
    public ChargedSkill()
    {
        Name = "Charged Attack";
        ManaCost = 25;
        Cooldown = 5;
        TargetType = TargetType.Enemy;
    }

    protected override void OnUse(Character caster, Character target)
    {
        int totalDamage = damage + caster.Attack;
        target.TakeDamage(totalDamage);
        Debug.Log(target.Name + " took " + totalDamage + " damage from " + caster.Name + "'s Charged Attack.");
    }
}