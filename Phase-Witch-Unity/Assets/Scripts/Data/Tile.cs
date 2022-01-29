using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public readonly Vector2Int Position;

    public Tile(Vector2Int pos)
    {
        Position = pos;
    }

    public Object Content;
}
