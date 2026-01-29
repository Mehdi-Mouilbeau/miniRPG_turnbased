using UnityEngine;

public class GuardSkill : Skill
{
    public int defenseIncrease;
    public int durationTurns;

    public GuardSkill()
    {
        Name = "Guard";
        ManaCost = 10;
        Cooldown = 4;
        TargetType = TargetType.Self;
        defenseIncrease = 10; 
        durationTurns = 3;
        Category = SkillCategory.Defensive;
    }

    protected override void OnUse(Character caster, Character target)
    {
        caster.DefenseBuff = defenseIncrease;
        caster.DefenseBuffTurns = durationTurns;
        Debug.Log(caster.Name + " increased defense by " + defenseIncrease + " for " + durationTurns + " turns using Guard.");
    }
}