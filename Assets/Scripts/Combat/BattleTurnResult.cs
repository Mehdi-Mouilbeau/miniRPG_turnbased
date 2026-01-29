public class BattleTurnResult
{
    public int TurnNumber { get; set; }
    public Character ActingCharacter { get; set; }
    public Character TargetCharacter { get; set; }
    public bool ActorIsPlayer { get; set; }
    public bool WasFrozen { get; set; }
    public bool WasBasicAttack { get; set; } 
    public Skill SkillUsed { get; set; }
    public bool GameEnded { get; set; }
}