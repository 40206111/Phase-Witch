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
