using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardBaseData 
{
    public int CardId;
    public string CardSpritePath;

    //view these as being the light side stats
    public int Damage;
    public int Health;

    public List<Ability> Abilities;


}
