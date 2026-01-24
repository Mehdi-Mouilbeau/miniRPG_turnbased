using UnityEngine;

public class Mage : Player
{
    public Mage()
    {
        Name = "Mage";
        MaxHP = 100;
        HP = MaxHP;
        MaxMana = 100;
        Mana = MaxMana;
        Attack = 30;
        Defense = 10;

        Skills.Add(new FireBallSkill());
        Skills.Add(new HealSkill());
        Skills.Add(new FrozenSkill());

        foreach (var s in Skills)
            Debug.Log("Mage skill: " + s.Name);

    }
}