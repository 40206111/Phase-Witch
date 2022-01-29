using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVGameBoard : MonoBehaviour
{
    public Vector2Int BoardSize = new Vector2Int(9, 4);
    public Transform TilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < BoardSize.y; ++y)
        {
            for (int x = 0; x < BoardSize.x; ++x)
            {
                Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
