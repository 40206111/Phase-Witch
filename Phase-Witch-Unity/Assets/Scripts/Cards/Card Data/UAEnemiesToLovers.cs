using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAEnemiesToLovers : SpellAbilityBaseCode
{
    public UAEnemiesToLovers(Ability numbers, ePhased phase) :
           base(numbers, phase)
    {
        NumOfTargets = 2;
    }

    protected override void SetTargettingDialogue()
    {
        if (Phase == ePhased.light)
        {
            TargettingDialogue = new List<string> { "Choose ally unit to heal", "Choose enemy unit to heal" };
        }
        else
        {
            TargettingDialogue = new List<string> { "Choose ally unit to harm", "Choose enemy unit to harm" };
        }
    }

    public override void DoSpell()
    {
        if (Phase == ePhased.light)
        {
            foreach (TilePiece piece in Targets) { 
                piece.DoHeal(Numbers.GetPhaseModifier(Phase), null);
            }
        }
        else
        {
            foreach (TilePiece piece in Targets)
            {
                piece.DoDamage(Numbers.GetPhaseModifier(Phase), null);
            }
        }
    }
}
