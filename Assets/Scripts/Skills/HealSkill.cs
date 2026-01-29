using UnityEngine;

public class HealSkill : Skill
{
    public int heal;
    public HealSkill()
    {
        Name = "Heal";
        ManaCost = 25;
        Cooldown = 3;
        TargetType = TargetType.Ally | TargetType.Self;
        Category = SkillCategory.Healing;
    }

    protected override void OnUse(Character caster, Character target)
    {
        target.Heal(heal);
        Debug.Log(target.Name + " healed " + heal + " HP from " + caster.Name + "'s Heal.");
    }
}