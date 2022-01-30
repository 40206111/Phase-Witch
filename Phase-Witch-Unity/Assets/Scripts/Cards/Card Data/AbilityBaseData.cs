using System;
using System.Collections.Generic;
using UnityEngine;

public enum eAbilityType
{
    None,
    ThornyRoses,
    BloodPact,
    HealthGain,
    HealAndDamage,
    EnemiesToLovers,
    DamageResistance,
}

public enum eActivationPoint
{
    Summon,
    Attack,
    Attacked
}

[Serializable]
public class AbilityBaseData 
{
    [Serializable]
    public struct SideSpecificAbilityData
    {
        public string AbilityName;
        public string AbilityDesc;
        public string AbilitySpritePath;
    }

    public eAbilityType AbilityType;
    public eActivationPoint ActivationTime;
    public SideSpecificAbilityData LightSideData;
    public SideSpecificAbilityData DarkSideData;


}
