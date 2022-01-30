using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellAbilityBaseCode
{
    public eAbilityType AbilityType;
    public Ability Numbers;
    public ePhased Phase;

    public int NumOfTargets = 0;
    public List<string> TargettingDialogue = new List<string>();
    protected List<TilePiece> Targets = new List<TilePiece>();

    public SpellAbilityBaseCode(Ability numbers, ePhased phase)
    {
        Numbers = numbers;
        AbilityType = numbers.AbilityType;
        Phase = phase;
    }

    public string GetSelectionDialogue()
    {
        if (TargettingDialogue.Count == 0)
        {
            return "Use spell";
        }
        return TargettingDialogue[0];
    }

    public string AddTarget(TilePiece piece)
    {
        Targets.Add(piece);
        if (Targets.Count < NumOfTargets)
        {
            return TargettingDialogue[Targets.Count];
        }
        else
        {
            return "";
        }
    }

    protected abstract void SetTargettingDialogue();

    public abstract void DoSpell();
}


public static class SpellFactory
{
    public static SpellAbilityBaseCode GetSpell(Ability ability, ePhased phase)
    {
        return ability.AbilityType switch
        {
            eAbilityType.HealAndDamage => new UAHealAndDamage(ability, phase),
            eAbilityType.EnemiesToLovers => new UAEnemiesToLovers(ability, phase),
            eAbilityType.DamageResistance => new UADamageResistance(ability, phase),
            _ => null
        };
    }
}
