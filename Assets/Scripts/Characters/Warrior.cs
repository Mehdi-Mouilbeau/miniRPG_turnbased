public class Warrior : Player
{
    public Warrior()
    {
        Name = "Warrior";
        MaxHP = 150;
        HP = MaxHP;
        MaxMana = 30;
        Mana = MaxMana;
        Attack = 25;
        Defense = 15;

        Skills.Add(new GuardSkill());
    }
}