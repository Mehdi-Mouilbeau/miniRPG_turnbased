using UnityEngine;

public class FireBallSkill : Skill
{

    public int damage;
    public FireBallSkill()
    {
        Name = "Fire Ball";
        damage = 20;
        ManaCost = 20;
        Cooldown = 2;
        TargetType = TargetType.Enemy;
        Category = SkillCategory.Offensive;
    }

    protected override void OnUse(Character caster, Character target)
    {
        int totalDamage = damage + caster.Attack;
        target.TakeDamage(totalDamage);
    }
}