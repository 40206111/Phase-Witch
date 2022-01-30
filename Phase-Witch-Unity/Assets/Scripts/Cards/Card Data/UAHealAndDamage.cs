using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAHealAndDamage : SpellAbilityBaseCode
{
    public UAHealAndDamage(Ability numbers, ePhased phase) :
           base(numbers, phase)
    {
        NumOfTargets = 1;
    }

    protected override void SetTargettingDialogue()
    {
        if (Phase == ePhased.light)
        {
            TargettingDialogue = new List<string>{ "Heal Target" };
        }
        else
        {
            TargettingDialogue = new List<string> { "Damage Target" };
        }
    }

    public override void DoSpell()
    {
        if(Phase == ePhased.light)
        {
            Targets[0].DoHeal(Numbers.GetPhaseModifier(Phase), null);
        }
        else
        {
            Targets[0].DoDamage(Numbers.GetPhaseModifier(Phase), null);
        }
    }
}
