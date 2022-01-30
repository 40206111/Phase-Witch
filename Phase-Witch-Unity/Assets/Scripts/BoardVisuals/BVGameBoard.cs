using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVGameBoard : MonoBehaviour
{
    private static BVGameBoard _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Double BVGameBoard Exists");
        }
        else
        {
            _instance = this;
        }
    }
    public static BVGameBoard Instance
    {
        get { return _instance; }
    }


    public Vector2Int BoardSize = new Vector2Int(9, 4);
    public Transform TilePrefab;
    public Transform PiecePrefab;

    private bool AwaitingSpecific = false;
    private List<Vector2Int> ValidTiles = new List<Vector2Int>();
    private Vector2Int? AwaitedTile = null;

    private BVTile[,] BVTiles;

    // Start is called before the first frame update
    void Start()
    {
        BVTiles = new BVTile[BoardSize.x,BoardSize.y];
        for (int y = 0; y < BoardSize.y; ++y)
        {
            for (int x = 0; x < BoardSize.x; ++x)
            {
                BVTiles[x, y] = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform).GetComponent<BVTile>();
            }
        }
        GameBoard.OnPieceSpawn += SpawnBVPiece;
        GameBoard.InitialiseBoard(BoardSize);
    }

    private void Update()
    {
        if(!AwaitingSpecific && Time.time > 1.0f)
        {
            StartCoroutine(RunTests());
        }
    }

    private IEnumerator RunTests()
    {
        yield return SpawnUnits();
    }

    private IEnumerator SpawnUnits()
    {
        for (int i = 0; i < 3; ++i)
        {
            AwaitingSpecific = true;
            AwaitedTile = null;
            ValidTiles = new List<Vector2Int>();

            for (int y = 0; y < GameBoard.BoardSize.y; ++y)
            {
                for (int x = 0; x < GameBoard.BoardSize.x; x += 3)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (!GameBoard.GetDataAtPos(pos).HasPiece && GameBoard.GetDataAtPos(pos).IsPassable)
                    {
                        ValidTiles.Add(pos);
                    }
                }
            }

            SetValidHighlighting(true);

            while (AwaitedTile == null)
            {
                yield return null;
            }

            SetValidHighlighting(false);

            GameBoard.SummonPiece(RayCaster.tempCard, AwaitedTile.Value); // ~~~
        }
    }

    private void SetValidHighlighting(bool state)
    {
        foreach (Vector2Int pos in ValidTiles)
        {
            BVTiles[pos.x, pos.y].SetHighlighting(state);
        }
    }

    private void SpawnBVPiece(TilePiece piece)
    {
        Transform bvPiece = Instantiate(PiecePrefab, EasyDir.Get3DFrom2D(piece.Position), Quaternion.identity, transform);
    }

    public void BVTileClicked(BVTile tile)
    {
        if (AwaitingSpecific && ValidTiles.Contains(tile.TilePos))
        {
            AwaitedTile = tile.TilePos;
        }
    }
}
