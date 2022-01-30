using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardDataLists
{
    public static int NextID = 0;

    public List<CardBaseData> AllCards = new List<CardBaseData>();

    public List<AbilityBaseData> AllAbilities = new List<AbilityBaseData>();
}

public static class CardDatabase
{



}
