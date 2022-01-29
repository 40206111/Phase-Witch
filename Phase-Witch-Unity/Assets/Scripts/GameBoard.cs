using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eDirection { none = -1, up, right, down, left }

public static class EasyDir
{
    public static readonly eDirection[] OrderedEnum =
        { eDirection.up, eDirection.right, eDirection.down, eDirection.left };
    public static readonly Vector2Int[] OrderedVectors =
        { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    public static Vector2Int DirFromEnum(eDirection dir)
    {
        return dir switch
        {
            eDirection.up => Vector2Int.up,
            eDirection.right => Vector2Int.right,
            eDirection.down => Vector2Int.down,
            eDirection.left => Vector2Int.left,
            _ => Vector2Int.zero
        };
    }

    public static eDirection EnumFromDir(Vector2Int dir)
    {
        return dir switch
        {
            Vector2Int { x: 0, y: 1 } => eDirection.up,
            Vector2Int { x: 1, y: 0 } => eDirection.right,
            Vector2Int { x: 0, y: -1 } => eDirection.down,
            Vector2Int { x: -1, y: 0 } => eDirection.left,
            _ => eDirection.none
        };
    }
}

public static class GameBoard
{
    public static List<List<object>> Board;
    public static Vector2Int BoardSize = Vector2Int.zero;

    public static void InitialiseBoard(Vector2Int size)
    {
        Board = new List<List<object>>(size.y);
        for (int i = 0; i < size.y; ++i)
        {
            Board[i] = new List<object>(size.x);
        }
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
        }

        return outList;
    }

    public static List<KeyValuePair<eDirection, Vector2Int>> GetNeighboursValid(Vector2Int pos)
    {
        List<KeyValuePair<eDirection, Vector2Int>> outList = new List<KeyValuePair<eDirection, Vector2Int>>();

        Vector2Int[] orderedList = GetNeighboursOrdered(pos);
        foreach (eDirection dir in EasyDir.OrderedEnum)
        {
            if (orderedList[(int)dir] != null)
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

    public static object GetDataAtPos(Vector2Int pos)
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
    private static object GetAtPos(Vector2Int pos)
    {
        return Board[pos.y][pos.x];
    }
}
