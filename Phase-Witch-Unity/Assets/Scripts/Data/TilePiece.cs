using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TilePiece : TileEntity
{
    public Action<TileEntity> OnAction;
    /// <summary>
    /// The affected entity, the value, the source.
    /// </summary>
    public Action<TileEntity, int, TileEntity> OnDamage;
    /// <summary>
    /// The affected entity, the value, the source.
    /// </summary>
    public Action<TileEntity, int, TileEntity> OnHealed;


    public UnitCardData CardData;

    public int MaxHealth;
    public int CurrentHealth;
    public int MaxDamage;
    public int CurrentDamage;

    public TilePiece(Vector2Int pos) : base(pos)
    {
        IsPassable = false;
    }

    public void Initialise(UnitCardData data)
    {
        MaxHealth = data.Health;
        CurrentHealth = MaxHealth;
        MaxDamage = data.Damage;
        CurrentDamage = MaxDamage;
        CardData = data;
    }

    public virtual void DoDamage(int val, TileEntity source)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - val, 0);
        OnDamage?.Invoke(this, val, source);
        if (CurrentHealth == 0)
        {
            OnEntityDeath?.Invoke(this, source);
        }
    }

    public virtual void DoHeal(int val, TileEntity source)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + val, MaxHealth); ;
        OnHealed?.Invoke(this, val, source);
    }

    public virtual void DoAction()
    {
        OnAction?.Invoke(this);
    }

}
