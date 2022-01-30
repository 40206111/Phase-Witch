using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAThornyRoses : UnitAbilityBaseCode
{
    public UAThornyRoses(TilePiece piece, Ability numbers) :
        base(piece, numbers)
    { }

    protected override void SignUpToPiece()
    {
        MyPiece.OnDamage += OnDamage;
    }

    protected virtual void OnDamage(TileEntity hurt, int val, TileEntity source)
    {
        if (source != hurt)
        {
            if (MyPiece.Phase == ePhased.dark)
            {
                if (source is TilePiece piece)
                {
                    piece.DoDamage(Numbers.GetPhaseModifier(MyPiece.Phase), piece);
                }
            }
            else
            {
                // ~~~ FOR NOW
                MyPiece.DoHeal(Numbers.GetPhaseModifier(MyPiece.Phase), MyPiece);
            }
        }
    }
}
