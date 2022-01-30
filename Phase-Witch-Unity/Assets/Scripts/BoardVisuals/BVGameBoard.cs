using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField]
    private RectTransform Button;
    private bool IsCardPhase = false;
    private CardController AwaitedCard = null;

    [SerializeField]
    private TextMeshProUGUI PromptText;
    private string OldPrompt = "";
    private string NewPrompt = "";
    private float TimeOfChange = -5.0f;
    private float TimeToFade = 5.0f;

    private eFaction ActiveFaction = eFaction.player;

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
        GameBoard.InitialiseBoard(BoardSize);

        PromptText.gameObject.SetActive(false);
        Button.gameObject.SetActive(false);

        StartCoroutine(RunTurns());
    }

    private void OnEnable()
    {
        GameBoard.OnPieceSpawn += SpawnBVPiece;
    }

    private void OnDisable()
    {
        GameBoard.OnPieceSpawn -= SpawnBVPiece;
    }

    private void Update()
    {
        if (NewPrompt != OldPrompt)
        {
            OldPrompt = NewPrompt;
            PromptText.text = NewPrompt;
            TimeOfChange = Time.time;
            PromptText.gameObject.SetActive(true);
        }

        if (PromptText.gameObject.activeInHierarchy && (TimeOfChange + TimeToFade <= Time.time))
        {
            PromptText.gameObject.SetActive(false);
        }
    }

    //private IEnumerator RunTests()
    //{
    //    yield return SpawnUnits();
    //    yield return MovePhase();
    //    yield return FacingPhase();
    //}

    //private IEnumerator SpawnUnits()
    //{
    //    for (int i = 0; i < 3; ++i)
    //    {
    //        ValidTiles = new List<Vector2Int>();

    //        for (int y = 0; y < GameBoard.BoardSize.y; ++y)
    //        {
    //            for (int x = 0; x < GameBoard.BoardSize.x; x += 3)
    //            {
    //                Vector2Int pos = new Vector2Int(x, y);
    //                if (!GameBoard.GetDataAtPos(pos).HasPiece && GameBoard.GetDataAtPos(pos).IsPassable)
    //                {
    //                    ValidTiles.Add(pos);
    //                }
    //            }
    //        }

    //        yield return StartCoroutine(AwaitTile());

    //        GameBoard.SummonPiece(RayCaster.tempCard, AwaitedTile.Value); // ~~~
    //    }
    //}


    private IEnumerator RunTurns()
    {
        yield return new WaitForSeconds(3.0f);
        bool running = true;
        while (running)
        {
            NewPrompt = "Your Turn";
            ActiveFaction = eFaction.player;
            // yield return StartCoroutine(SpawnUnits());
            yield return StartCoroutine(MovePhase());
            yield return StartCoroutine(FacingPhase());
            yield return StartCoroutine(CardPhase());
            yield return StartCoroutine(ActionPhase());

            NewPrompt = "Enemy Turn";
            ActiveFaction = eFaction.enemy;
            //yield return StartCoroutine(CardPhase());
            yield return StartCoroutine(MovePhase());
            yield return StartCoroutine(FacingPhase());
            yield return StartCoroutine(ActionPhase());
        }
    }

    private IEnumerator CardPhase()
    {
        NewPrompt = "Card Phase";

        IsCardPhase = true;
        Button.gameObject.SetActive(true);
        while (IsCardPhase)
        {
            AwaitedCard = null;
            while (IsCardPhase && AwaitedCard == null)
            {
                yield return null;
            }
            if (!IsCardPhase)
            {
                break;
            }

            ValidTiles = new List<Vector2Int>();
            bool isUnit = false;
            bool useCard = true;
            SpellAbilityBaseCode spell = null;
            // populate valid
            if (AwaitedCard.Card is UnitCardData unitCard)
            {
                isUnit = true;
                for (int y = 0; y < GameBoard.BoardSize.y; ++y)
                {
                    for (int x = 0; x < GameBoard.BoardSize.x; ++x)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        if (GameBoard.GetDataAtPos(pos).IsPassable)
                        {
                            ValidTiles.Add(pos);
                        }
                    }
                }
            }
            else
            {
                unitCard = null;
                spell = SpellFactory.GetSpell(AwaitedCard.Card.Abilities[0], AwaitedCard.IsLight ? ePhased.light : ePhased.dark);
                if (spell.NumOfTargets > GameBoard.AllPieces.Count)
                {
                    useCard = false;
                }
                else
                {
                    for (int y = 0; y < GameBoard.BoardSize.y; ++y)
                    {
                        for (int x = 0; x < GameBoard.BoardSize.x; ++x)
                        {
                            Vector2Int pos = new Vector2Int(x, y);
                            if (GameBoard.GetDataAtPos(pos).HasPiece)
                            {
                                ValidTiles.Add(pos);
                            }
                        }
                    }
                }
            }

            if (!useCard)
            {
                continue;
            }

            if (isUnit)
            {
                NewPrompt = "Choose Spawn Point";
            }
            else
            {
                NewPrompt = spell.GetSelectionDialogue();
            }
            yield return StartCoroutine(AwaitTile());

            if (isUnit)
            {
                GameBoard.SummonPiece(unitCard, AwaitedCard.IsLight ? ePhased.light : ePhased.dark, ActiveFaction,
                    AwaitedTile.Value);
            }
            else
            {
                List<Vector2Int> usedTargets = new List<Vector2Int>() { AwaitedTile.Value };
                string prompt = spell.AddTarget(GameBoard.GetDataAtPos(AwaitedTile.Value).Piece);
                while (prompt != "")
                {
                    ValidTiles = new List<Vector2Int>();
                    for (int y = 0; y < GameBoard.BoardSize.y; ++y)
                    {
                        for (int x = 0; x < GameBoard.BoardSize.x; ++x)
                        {
                            Vector2Int pos = new Vector2Int(x, y);
                            if (GameBoard.GetDataAtPos(pos).HasPiece && !usedTargets.Contains(pos))
                            {
                                ValidTiles.Add(pos);
                            }
                        }
                    }

                    NewPrompt = prompt;
                    yield return StartCoroutine(AwaitTile());
                    prompt = spell.AddTarget(GameBoard.GetDataAtPos(AwaitedTile.Value).Piece);
                }
                spell.DoSpell();
            }

            Destroy(AwaitedCard.gameObject, 0.05f);
        }
        IsCardPhase = false;
        Button.gameObject.SetActive(false);
        AwaitedCard = null;
    }

    private IEnumerator MoveAI()
    {
        yield return null;
    }

    private IEnumerator MovePhase()
    {
        NewPrompt = "Move Phase";

        if (ActiveFaction == eFaction.enemy)
        {
            yield return StartCoroutine(MoveAI());
            yield break;
        }

        List<TilePiece> potentialUnits = new List<TilePiece>();
        for (int y = 0; y < GameBoard.BoardSize.y; ++y)
        {
            for (int x = 0; x < GameBoard.BoardSize.x; ++x)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (GameBoard.GetDataAtPos(pos).HasPiece &&
                    GameBoard.GetDataAtPos(pos).Piece.Faction == ActiveFaction)
                {
                    potentialUnits.Add(GameBoard.GetDataAtPos(pos).Piece);
                }
            }
        }

        for (int i = 0; i < 2 && potentialUnits.Count != 0; ++i)
        {
            #region choose piece to move
            ValidTiles = new List<Vector2Int>();
            foreach (TilePiece piece in potentialUnits)
            {
                ValidTiles.Add(piece.Position);
            }
            yield return StartCoroutine(AwaitTile());
            #endregion

            #region step through selected piece's moves
            bool abort = false;
            TilePiece selected = GameBoard.GetDataAtPos(AwaitedTile.Value).Piece;
            for (int m = 0; m < selected.MoveSpeed && !abort; ++m)
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
                    if (m == 0)
                    {
                        abort = true;
                    }
                    break;
                }
                else
                {
                    BVPiece bvPiece = BVTiles[selected.Position.x, selected.Position.y].RemovePiece();
                    GameBoard.MovePiece(selected, AwaitedTile.Value);
                    BVTiles[AwaitedTile.Value.x, AwaitedTile.Value.y].AddPiece(bvPiece);
                }
            }
            if (abort)
            {
                --i;
            }
            else
            {
                potentialUnits.Remove(selected);
            }
            #endregion
        }
    }

    private IEnumerator FacingAI()
    {
        yield return null;
    }
    private IEnumerator FacingPhase()
    {
        NewPrompt = "Rotate Phase";

        if (ActiveFaction == eFaction.enemy)
        {
            yield return StartCoroutine(FacingAI());
            yield break;
        }

        List<TilePiece> potentialUnits = new List<TilePiece>();
        for (int y = 0; y < GameBoard.BoardSize.y; ++y)
        {
            for (int x = 0; x < GameBoard.BoardSize.x; ++x)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (GameBoard.GetDataAtPos(pos).HasPiece &&
                    GameBoard.GetDataAtPos(pos).Piece.Faction == ActiveFaction)
                {
                    potentialUnits.Add(GameBoard.GetDataAtPos(pos).Piece);
                }
            }
        }
        // ~~~ repeat for all units
        while (potentialUnits.Count > 0)
        {
            #region choose piece to move
            ValidTiles = new List<Vector2Int>();
            foreach (TilePiece piece in potentialUnits)
            {
                ValidTiles.Add(piece.Position);
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
            if (!(AwaitedTile.Value == selected.Position || facingDir == EasyDir.DirFromEnum(selected.Facing)))
            {
                selected.ChangeFacing(EasyDir.EnumFromDir(facingDir));
                BVTiles[selected.Position.x, selected.Position.y].BVPiece.RotateFacing(EasyDir.EnumFromDir(facingDir));
            }
            potentialUnits.Remove(selected);
        }
    }

    private IEnumerator ActionAI()
    {
        yield return null;
    }
    private IEnumerator ActionPhase()
    {
        NewPrompt = "Action Phase";

        if (ActiveFaction == eFaction.enemy)
        {
            yield return StartCoroutine(MoveAI());
            yield break;
        }

        yield return null;
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

    public void CardPlayed(CardController card)
    {
        if (AwaitedCard != null)
        {
            return;
        }
        AwaitedCard = card;
    }

    public void EndCardPhaseClicked()
    {
        IsCardPhase = false;
    }
}
