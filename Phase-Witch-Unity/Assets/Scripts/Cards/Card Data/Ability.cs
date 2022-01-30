using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ability
{

    public eAbilityType AbilityType;
    public int LightModifier;
    public int DarkModifier;

    public int GetPhaseModifier(ePhased phase)
    {
        return phase switch
        {
            ePhased.light => LightModifier,
            ePhased.dark => DarkModifier,
            _ => 0
        };
    }
}
