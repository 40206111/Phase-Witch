using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVTile : MonoBehaviour
{
    public Vector2Int TilePos;
    private float progress; // 0 = black, 1 = white
    private bool _isHighlighted = false;
    Material Material;

    public BVPiece BVPiece;

    public bool Odd;

    // Start is called before the first frame update
    void Start()
    {
        TilePos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        Odd = ((TilePos.x + TilePos.y) % 2) == 1;
        Material = GetComponentInChildren<MeshRenderer>().material;
        SetHighlighting(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHighlighted)
        {
            UpdateHighlighting();
        }
    }

    public void SetHighlighting(bool state)
    {
        if (state)
        {
            UpdateHighlighting();
        }
        else
        {
            progress = Odd ? 0 : 1;
            SetMaterialColour(progress);
        }
        _isHighlighted = state;
    }

    private void UpdateHighlighting()
    {
        // float halfHeight = 0.4f;
        progress = (Mathf.Sin(Time.time * Mathf.PI) * 0.15f) + 0.25f;
        progress = !Odd ? progress + 0.5f : progress;
        SetMaterialColour(progress);
    }

    private void SetMaterialColour(float val)
    {
        // Debug.Log(val);
        Material.color = new Color(val, val, val);
    }

    public void Clicked()
    {
        //progress = 0.0f;
        BVGameBoard.Instance.BVTileClicked(this);
    }

    public void AddPiece(BVPiece piece)
    {
        BVPiece = piece;
        BVPiece.transform.position = transform.position;
    }

    public BVPiece RemovePiece()
    {
        BVPiece outPiece = BVPiece;
        BVPiece = null;
        return outPiece;
    }
}
