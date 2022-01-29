using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TilePiece : TileEntity
{
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxDamage;
    public int CurrentDamage;

    public TilePiece(Vector2Int pos) : base(pos)
    {
        IsPassable = false;
    }

    public abstract void OnAction();
    public abstract void OnDamage();
    public abstract void OnHealed();
}
