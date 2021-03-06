using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameBoard
{

    public static Action<TilePiece, Vector2Int> OnPieceEnter;
    public static Action<TilePiece, Vector2Int> OnPieceLeave;
    public static Action<TilePiece, Sprite> OnPieceSpawn;
    public static Action<TilePiece, TilePiece> OnPieceDeath;
    public static Action<TilePiece> OnPieceAction;
    public static Action<TilePiece, int, TilePiece> OnPieceDamaged;
    public static Action<TilePiece, int, TilePiece> OnPieceHealed;

    public static Tile[,] Board;
    public static Vector2Int BoardSize = Vector2Int.zero;

    public static List<TilePiece> AllPieces = new List<TilePiece>();

    private static bool FirstTime = true;

    public static void InitialiseBoard(Vector2Int size)
    {
        if (FirstTime)
        {
            OnPieceSpawn += OnPieceSpawnBoard;
            OnPieceDeath += OnPieceDeathBoard;
        }

        Board = new Tile[size.x, size.y];
        for (int j = 0; j < size.y; ++j)
        {
            for (int i = 0; i < size.x; ++i)
            {
                Board[i, j] = new Tile(new Vector2Int(i, j));
            }
        }
        BoardSize = size;
    }

    public static Vector2Int[] GetNeighboursOrdered(Vector2Int pos)
    {
        Vector2Int[] outList = new Vector2Int[4];

        foreach (eDirection dir in EasyDir.OrderedEnum)
        {
            Vector2Int testPos = pos + EasyDir.DirFromEnum(dir);
            if (IsValidPos(testPos))
            {
                outList[(int)dir] = testPos;
            }
            else
            {
                outList[(int)dir] = BoardSize;
            }
        }

        return outList;
    }

    public static List<KeyValuePair<eDirection, Vector2Int>> GetNeighboursValid(Vector2Int pos)
    {
        List<KeyValuePair<eDirection, Vector2Int>> outList = new List<KeyValuePair<eDirection, Vector2Int>>();

        Vector2Int[] orderedList = GetNeighboursOrdered(pos);
        foreach (eDirection dir in EasyDir.OrderedEnum)
        {
            if (orderedList[(int)dir] != BoardSize)
            {
                outList.Add(new KeyValuePair<eDirection, Vector2Int>(dir, orderedList[(int)dir]));
            }
        }

        return outList;
    }

    public static bool IsValidPos(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < BoardSize.x && pos.y >= 0 && pos.y < BoardSize.y)
        {
            return true;
        }
        return false;
    }

    public static Tile GetDataAtPos(Vector2Int pos)
    {
        if (IsValidPos(pos))
        {
            return GetAtPos(pos);
        }
        return null;
    }

    /// <summary>
    /// UNSAFE method that doesn't check for valid pos.  Hides steps of accessing board from more general functions for ease of use.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private static Tile GetAtPos(Vector2Int pos)
    {
        return Board[pos.x, pos.y];
    }

    public static bool SummonPiece(UnitCardData cardData, ePhased phase, eFaction faction, Vector2Int pos)
    {
        bool outBool = false;

        if (IsValidPos(pos))
        {
            Tile tile = GetAtPos(pos);
            // If tile can hold new piece
            if (!tile.HasPiece && !(tile.HasEffect && !tile.Effect.IsPassable))
            {
                TilePiece piece = new TilePiece(pos);
                piece.Initialise(cardData, phase, faction);
                tile.Piece = piece;

                string side = phase == ePhased.light ? "Light" : "Dark";
                string path = $"piece/{side}/{cardData.CardSpritePath}";
                Debug.Log(path);
                var sprite = Resources.Load<Sprite>(path);
                Debug.Log(sprite);
                OnPieceSpawn?.Invoke(piece, sprite);//~~~
                outBool = true;
            }
        }

        return outBool;
    }

    public static bool MovePiece(TilePiece piece, Vector2Int target)
    {
        if (!IsValidPos(target))
        {
            return false;
        }
        OnPieceLeave?.Invoke(piece, piece.Position);
        GetDataAtPos(piece.Position).Piece = null;
        piece.ChangePosition(target);
        GetDataAtPos(target).Piece = piece;
        OnPieceEnter?.Invoke(piece, piece.Position);
        return true;
    }

    public static void OnPieceSpawnBoard(TilePiece piece, Sprite sprite = null)
    {
        AllPieces.Add(piece);
    }

    public static void OnPieceDeathBoard(TilePiece piece, TilePiece killer)
    {
        if (AllPieces.Contains(piece))
        {
            AllPieces.Remove(piece);
        }
    }
}
