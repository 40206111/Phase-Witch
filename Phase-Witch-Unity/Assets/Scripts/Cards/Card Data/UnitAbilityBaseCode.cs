using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAbilityBaseCode
{
    public eAbilityType AbilityType;
    protected TilePiece MyPiece;
    public Ability Numbers;
    public UnitAbilityBaseCode(TilePiece piece, Ability numbers)
    {
        MyPiece = piece;
        Numbers = numbers;
        AbilityType = numbers.AbilityType;
        SignUpToPiece();
    }

    protected abstract void SignUpToPiece();
}
