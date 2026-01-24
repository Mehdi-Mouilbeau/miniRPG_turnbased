using UnityEngine;

public class SkillTest : MonoBehaviour
{
    void Start()
    {
        Warrior warrior = new Warrior();
        Mage mage = new Mage();

        mage.Skills[0].Use(mage, warrior); // Mage uses Fire Ball on Warrior
        warrior.Skills[0].Use(warrior, warrior); // Warrior uses Shield Bash on self
        mage.Skills[0].Use(mage, mage);

        foreach (var skill in mage.Skills)
        {
            skill.ReduceCooldown();
        }

        mage.Skills[0].Use(mage, warrior); // Mage tries to use Fire Ball again
    }
}