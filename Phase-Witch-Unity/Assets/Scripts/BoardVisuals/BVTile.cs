using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVTile : MonoBehaviour
{
    public Vector2Int TilePos;
    private float progress;
    // Start is called before the first frame update
    void Start()
    {
        TilePos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (progress < 1.0f)
        {
            progress = Mathf.Clamp(progress + Time.deltaTime, 0, 1);

            GetComponentInChildren<MeshRenderer>().material.color = new Color(progress, progress, progress);
        }
    }

    public void Clicked()
    {
        progress = 0.0f;
    }
}
