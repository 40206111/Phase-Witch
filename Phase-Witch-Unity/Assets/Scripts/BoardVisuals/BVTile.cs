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

    // Start is called before the first frame update
    void Start()
    {
        TilePos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        Material = GetComponentInChildren<MeshRenderer>().material;
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
            progress = 1;
            SetMaterialColour(progress);
        }
        _isHighlighted = state;
    }

    private void UpdateHighlighting()
    {
        // float halfHeight = 0.4f;
        progress = (Mathf.Sin(Time.time * Mathf.PI) * 0.15f) + 0.2f;
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
