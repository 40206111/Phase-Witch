using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVPiece : MonoBehaviour
{
    [SerializeField]
    Transform FacingMarker;

    public void RotateFacing(eDirection dir)
    {
        FacingMarker.rotation = Quaternion.Euler(FacingMarker.rotation.eulerAngles.x, 90.0f * (float)dir, 0.0f);
    }
}
