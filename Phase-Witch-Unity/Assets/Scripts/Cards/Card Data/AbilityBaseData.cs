using System;
using System.Collections.Generic;
using UnityEngine;

public enum eAbilityType
{
    None,
    ThornyRoses,
}

[Serializable]
public class AbilityBaseData 
{
    public struct SideSpecificAbilityData
    {
        string AbilityName;
        string AbilityDesc;
        Sprite AbilitySprite;
    }


    public SideSpecificAbilityData LightSideData;
    public SideSpecificAbilityData DarkSideData;


}
