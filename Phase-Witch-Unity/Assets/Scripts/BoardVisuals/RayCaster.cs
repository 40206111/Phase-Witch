using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoMouseClick();
        }
    }

    private void DoMouseClick()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            BVTile tile = hit.collider.GetComponentInParent<BVTile>();
            if (tile != null)
            {
                Debug.Log(Time.time + " " + tile.TilePos);
                tile.Clicked();
            }
        }
    }
}
