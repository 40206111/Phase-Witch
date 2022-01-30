using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEffect : TileEntity
{
    public TileEffect(Vector2Int pos) : base(pos)
    {
        IsPassable = true;
        GameBoard.OnPieceEnter += OnPieceEnter;
    }

    public virtual void OnPieceEnter(TilePiece piece, Vector2Int pos)
    {
        if (pos == Position)
        {
            DoEffect(piece);
        }
    }

    public abstract void DoEffect(TilePiece piece);
}
