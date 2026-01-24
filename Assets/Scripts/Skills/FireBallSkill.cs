using UnityEngine;

public class FireBallSkill : Skill
{

    public int damage;
    public FireBallSkill()
    {
        Name = "Fire Ball";
        damage = 40;
        ManaCost = 20;
        Cooldown = 2;
        TargetType = TargetType.Enemy;
    }

    protected override void OnUse(Character caster, Character target)
    {
        int totalDamage = damage + caster.Attack;
        target.TakeDamage(totalDamage);
    }
}