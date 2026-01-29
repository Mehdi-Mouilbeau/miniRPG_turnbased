public class DefaultAIStrategy : IAIStrategy
{
    public Skill ChooseAction(Character actor, Character target)
    {
        // 1) Défense si pas de buff actif
        Skill defensive = FindSkillByCategory(actor, SkillCategory.Defensive);
        if (defensive != null && defensive.CanUse(actor) && actor.DefenseBuffTurns == 0)
        {
            return defensive;
        }

        // 2) Soin si HP < 50%
        Skill healing = FindSkillByCategory(actor, SkillCategory.Healing);
        if (healing != null && healing.CanUse(actor) && actor.HP < actor.MaxHP / 2)
        {
            return healing;
        }

        // 3) Offensive prioritaire
        Skill offensive = ChooseBestOffensiveSkill(actor);
        if (offensive != null)
        {
            return offensive;
        }

        return null; // Attaque basique
    }

    private Skill FindSkillByCategory(Character character, SkillCategory category)
    {
        if (character.Skills == null) return null;

        foreach (Skill skill in character.Skills)
        {
            if (skill.Category == category && skill.CanUse(character))
            {
                return skill;
            }
        }
        return null;
    }

    private Skill ChooseBestOffensiveSkill(Character actor)
    {
        if (actor.Skills == null) return null;

        Skill best = null;

        foreach (Skill skill in actor.Skills)
        {
            if (skill.Category != SkillCategory.Offensive) continue;
            if (skill.TargetType != TargetType.Enemy) continue;
            if (!skill.CanUse(actor)) continue;

            if (best == null ||
                skill.Cooldown > best.Cooldown ||
                (skill.Cooldown == best.Cooldown && skill.ManaCost > best.ManaCost))
            {
                best = skill;
            }
        }

        return best;
    }
}