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
    List<UnitAbilityBaseCode> Abilities = new List<UnitAbilityBaseCode>();

    public int MaxHealth;
    public int CurrentHealth;
    public int MaxDamage;
    public int CurrentDamage;

    public int MoveSpeed = 3;

    public TilePiece(Vector2Int pos) : base(pos)
    {
        IsPassable = false;
    }

    public void Initialise(UnitCardData data, ePhased playPhase, eFaction faction)
    {
        Phase = playPhase;
        if (Phase == ePhased.light)
        {
            MaxHealth = data.Health;
            MaxDamage = data.Damage;
        }
        else
        {
            MaxHealth = data.Damage;
            MaxDamage = data.Health;
        }
            
        CurrentHealth = MaxHealth;
        CurrentDamage = MaxDamage;
        Faction = faction;
        CardData = data;
        foreach(Ability ab in data.Abilities)
        {
            Abilities.Add(CreateAbility(ab));
        }

        OnEntitySpawn?.Invoke(this);
    }

    protected UnitAbilityBaseCode CreateAbility(Ability ab)
    {
        return ab.AbilityType switch
        {
            eAbilityType.HealthGain => new UAMaxHealthOnSpawn(this, ab),
            eAbilityType.ThornyRoses => new UAThornyRoses(this, ab),
            eAbilityType.BloodPact => new UABloodPact(this, ab),
            _ => null
        };
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

    public virtual void SetMaxHealth(int newVal, TileEntity source)
    {
        int oldVal = MaxHealth;
        MaxHealth = newVal;

        int diff = newVal - oldVal;
        if(Mathf.Sign(diff) > 0)
        {
            DoHeal(diff, source);
        }
        else
        {
            if(CurrentHealth > MaxHealth)
            {
                DoDamage(CurrentHealth - MaxHealth, source);
            }
        }

    }
}
