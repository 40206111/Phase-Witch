using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEntity
{
    protected Vector2Int _position;
    public Vector2Int Position { get { return _position; } }
    public bool IsPassable;
    protected eDirection _facing;
    public eDirection Facing { get { return _facing; } }

    public TileEntity(Vector2Int pos)
    {
        _position = pos;
        OnEntitySpawn();
        OnTileEnter();
    }

    public virtual void ChangePosition(Vector2Int newPos)
    {
        OnTileLeave();
        _position = newPos;
        OnTileEnter();
    }

    public abstract void OnEntitySpawn();
    public abstract void OnTileLeave();
    public abstract void OnTileEnter();
    public abstract void OnEntityDeath();
    public abstract void OnFacingChanged();
}
