using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class TileEntity
{
    public Action<TileEntity> OnEntitySpawn;
    public Action<TileEntity, Vector2Int> OnTileLeave;
    public Action<TileEntity, Vector2Int> OnTileEnter;
    /// <summary>
    /// The dead entity, the killer.
    /// </summary>
    public Action<TileEntity, TileEntity> OnEntityDeath;
    public Action<TileEntity> OnFacingChanged;


    public int Faction = -1;
    protected Vector2Int _position;
    public Vector2Int Position { get { return _position; } }
    public bool IsPassable;
    protected eDirection _facing;
    public eDirection Facing { get { return _facing; } }

    public TileEntity(Vector2Int pos)
    {
        _position = pos;
        OnEntitySpawn.Invoke(this);
        OnTileEnter(this, pos);
    }

    public virtual void ChangePosition(Vector2Int newPos)
    {
        OnTileLeave(this, Position);
        _position = newPos;
        OnTileEnter(this, Position);
    }

}
