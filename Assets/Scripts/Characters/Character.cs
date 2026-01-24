using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name;
    public int MaxHP;
    public int HP;
    public int MaxMana;
    public int Mana;
    public int Attack;
    public int Defense;
    public int DefenseBuff;
    public int DefenseBuffTurns;

    public bool IsFrozen;

    public int FrozenTurns;


    public List<Skill> Skills = new List<Skill>();

    public bool IsAlive() { return HP > 0; }

    //  Take damage method
    public virtual void TakeDamage(int damage)
    {
        int effectiveDefense = Defense + DefenseBuff;
        int finalDamage = Mathf.Max(1, damage - effectiveDefense);
        HP -= finalDamage;
        if (HP < 0) HP = 0;
        Debug.Log(Name + " took " + finalDamage + " damage. Remaining HP: " + HP);
    }

    // Heal method
    public void Heal(int amount)
    {
        HP = Mathf.Min(MaxHP, HP + amount);
        Debug.Log(Name + " healed " + amount + " HP. Current HP: " + HP);
    }


    // Call this method at the end of each turn to update buff durations
    public void TickEffects()
    {
        // Defeense buff
        if (DefenseBuffTurns > 0)
        {
            DefenseBuffTurns--;
            if (DefenseBuffTurns == 0)
            {
                DefenseBuff = 0;
                Debug.Log(Name + " n'a plus de buff défense.");
            }
        }
        // Frozen effect
        if (IsFrozen)
        {
            FrozenTurns--;
            if (FrozenTurns <= 0)
            {
                IsFrozen = false;
                Debug.Log(Name + " is no longer frozen.");
            }
        }
    }

    // Apply a temporary defense buff
    public void ApplyDefenseBuff(int amount, int turns)
    {
        DefenseBuff += amount;
        DefenseBuffTurns = turns;
        Debug.Log(Name + " gained a defense buff of " + amount + " for " + turns + " turns.");
    }




}