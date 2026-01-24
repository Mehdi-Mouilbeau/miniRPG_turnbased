using UnityEngine;

public enum TargetType
{
    Self,
    Ally,
    Enemy
}

public abstract class Skill
{
    public string Name { get; protected set; }
    public int ManaCost { get; protected set; }
    public int Cooldown { get; protected set; }
    public int CurrentCooldown { get; private set; }
    public TargetType TargetType { get; protected set; }

    // Effect could be a delegate or a method reference, depending on implementation
    // public abstract void Use(GameObject user, GameObject target);

    // Spell on cd 
    public bool IsOnCooldown()
    {
        return CurrentCooldown > 0;
    }

    // Can the caster use the spell
    public bool CanUse(Character caster)
    {
        if (caster == null)
            return false;
        if (caster.Mana < ManaCost)
            return false;
        if (IsOnCooldown())
            return false;
        return true;
    }

    // Start the cooldown
    public void StartCooldown()
    {
        CurrentCooldown = Cooldown;
    }

    // Reduce the cooldown by 1
    public void ReduceCooldown()
    {
        if (CurrentCooldown > 0)
            CurrentCooldown--;
    }

    // Reset the cooldown to 0
    public void ResetCooldown()
    {
        CurrentCooldown = 0;
    }

    // Use the skill
    public void Use(Character caster, Character target)
    {
        // Check if can use
        if (!CanUse(caster))
        {
            Debug.Log(caster.Name + " cannot use " + Name + " right now.");
            return;
        }

        // Determine target
        if (TargetType == TargetType.Self) target = caster;
        if (target == null)
        {
            Debug.Log("No valid target for " + Name + ".");
            return;
        }


        // Deduct mana
        caster.Mana -= ManaCost;
        Debug.Log(caster.Name + " used " + Name + " on " + target.Name + ".");

        OnUse(caster, target);
        // Start cooldown
        StartCooldown();
    }
    protected abstract void OnUse(Character caster, Character target);

}