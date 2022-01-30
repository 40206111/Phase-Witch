using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAMaxHealthOnSpawn : UnitAbilityBaseCode
{
    public UAMaxHealthOnSpawn(TilePiece piece, Ability numbers) :
        base(piece, numbers)
    { }

    protected override void SignUpToPiece()
    {
        MyPiece.OnEntitySpawn += GiveBuff;
    }

    protected virtual void GiveBuff(TileEntity piece)
    {
        MyPiece.SetMaxHealth(MyPiece.MaxHealth + Numbers.GetPhaseModifier(MyPiece.Phase), MyPiece);
    }
}
