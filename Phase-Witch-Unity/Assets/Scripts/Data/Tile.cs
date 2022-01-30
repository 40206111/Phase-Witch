using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public readonly Vector2Int Position;
    public bool _isPassable = true;

    public TilePiece Piece;
    public TileEffect Effect;

    public Tile(Vector2Int pos)
    {
        Position = pos;
    }

    public void SetPassable(bool state)
    {
        _isPassable = state;
    }

    /// <summary>
    /// If the tile itself is passable. Does not consider contents.
    /// </summary>
    /// <returns></returns>
    public bool GetTilePassable()
    {
        return _isPassable;
    }

    /// <summary>
    /// If the tile and its contents can be passed through.
    /// </summary>
    public bool IsPassable
    {
        get
        {
            bool outBool = _isPassable;

            if (HasPiece)
            {
                outBool = outBool && Piece.IsPassable;
            }
            if (HasEffect)
            {
                outBool = outBool && Effect.IsPassable;
            }

            return outBool;
        }
    }

    public bool HasPiece
    {
        get
        {
            return Piece != null;
        }
    }

    public bool HasEffect
    {
        get
        {
            return Effect != null;
        }
    }
}
