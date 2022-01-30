using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFaction { none = -1, player, enemy }
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

    private int ActiveFaction = (int)eFaction.player;

    private BVTile[,] BVTiles;

    // Start is called before the first frame update
    void Start()
    {
        BVTiles = new BVTile[BoardSize.x, BoardSize.y];
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
        if (!AwaitingSpecific && Time.time > 1.0f)
        {
            StartCoroutine(RunTests());
        }
    }

    private IEnumerator RunTests()
    {
        yield return SpawnUnits();
        yield return MoveUnits();
        yield return FaceUnits();
    }

    private IEnumerator SpawnUnits()
    {
        for (int i = 0; i < 3; ++i)
        {
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

            yield return StartCoroutine(AwaitTile());

            GameBoard.SummonPiece(RayCaster.tempCard, AwaitedTile.Value); // ~~~
        }
    }

    private IEnumerator MoveUnits()
    {
        for (int i = 0; i < 2; ++i)
        {
            #region choose piece to move
            ValidTiles = new List<Vector2Int>();
            for (int y = 0; y < GameBoard.BoardSize.y; ++y)
            {
                for (int x = 0; x < GameBoard.BoardSize.x; ++x)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (GameBoard.GetDataAtPos(pos).HasPiece && GameBoard.GetDataAtPos(pos).Piece.Faction == ActiveFaction)
                    {
                        ValidTiles.Add(pos);
                    }
                }
            }

            yield return StartCoroutine(AwaitTile());
            #endregion

            #region step through selected piece's moves
            bool abort = false;
            TilePiece selected = GameBoard.GetDataAtPos(AwaitedTile.Value).Piece;
            for (int m = 0; m < selected.MoveSpeed; ++m)
            {
                ValidTiles = new List<Vector2Int>();
                foreach (var pair in GameBoard.GetNeighboursValid(selected.Position))
                {
                    if (GameBoard.GetDataAtPos(pair.Value).IsPassable)
                    {
                        ValidTiles.Add(pair.Value);
                    }
                    ValidTiles.Add(selected.Position);
                }

                yield return StartCoroutine(AwaitTile());

                if (AwaitedTile.Value == selected.Position)
                {
                    break;
                }
                else
                {
                    BVPiece bvPiece = BVTiles[selected.Position.x, selected.Position.y].RemovePiece();
                    GameBoard.MovePiece(selected, AwaitedTile.Value);
                    BVTiles[AwaitedTile.Value.x, AwaitedTile.Value.y].AddPiece(bvPiece);
                }
            }
            #endregion
        }
    }

    private IEnumerator FaceUnits()
    {
        // ~~~ repeat for all units
        for (int i = 0; i < 2; ++i)
        {
            #region choose piece to move
            ValidTiles = new List<Vector2Int>();
            for (int y = 0; y < GameBoard.BoardSize.y; ++y)
            {
                for (int x = 0; x < GameBoard.BoardSize.x; ++x)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (GameBoard.GetDataAtPos(pos).HasPiece && GameBoard.GetDataAtPos(pos).Piece.Faction == ActiveFaction)
                    {
                        ValidTiles.Add(pos);
                    }
                }
            }
            yield return StartCoroutine(AwaitTile());
            #endregion

            TilePiece selected = GameBoard.GetDataAtPos(AwaitedTile.Value).Piece;
            ValidTiles = new List<Vector2Int>();
            foreach (var pair in GameBoard.GetNeighboursValid(selected.Position))
            {
                ValidTiles.Add(pair.Value);
                ValidTiles.Add(selected.Position);
            }

            yield return StartCoroutine(AwaitTile());

            Vector2Int facingDir = AwaitedTile.Value - selected.Position;
            if (AwaitedTile.Value == selected.Position || facingDir == EasyDir.DirFromEnum(selected.Facing))
            {
                continue;
            }
            else
            {
                selected.ChangeFacing(EasyDir.EnumFromDir(facingDir));
                BVTiles[selected.Position.x, selected.Position.y].BVPiece.RotateFacing(EasyDir.EnumFromDir(facingDir));
            }
        }
    }

    private IEnumerator AwaitTile()
    {
        AwaitingSpecific = true;
        AwaitedTile = null;

        SetValidHighlighting(true);

        while (AwaitedTile == null)
        {
            yield return null;
        }

        SetValidHighlighting(false);
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
        BVTiles[piece.Position.x, piece.Position.y].BVPiece = Instantiate(PiecePrefab, EasyDir.Get3DFrom2D(piece.Position), Quaternion.identity, transform).GetComponent<BVPiece>();
    }

    public void BVTileClicked(BVTile tile)
    {
        if (AwaitingSpecific && ValidTiles.Contains(tile.TilePos))
        {
            AwaitedTile = tile.TilePos;
        }
    }
}
