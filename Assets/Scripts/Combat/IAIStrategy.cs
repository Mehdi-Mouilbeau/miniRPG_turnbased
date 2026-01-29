public interface IAIStrategy
{
    Skill ChooseAction(Character actor, Character target);
}