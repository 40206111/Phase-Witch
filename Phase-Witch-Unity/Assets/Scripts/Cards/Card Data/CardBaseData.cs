using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardBaseData 
{
    public int CardId;
    public string CardSpritePath;

    public List<Ability> Abilities;

    public bool EnemyCard;


}
