using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUserInteraction : MonoBehaviour
{
    public static float LastHoverTime = -5.0f;
    GraphicRaycaster gRay;
    PointerEventData ped;
    EventSystem eSys;

    CardController CardCon;
    // Start is called before the first frame update
    void Start()
    {
        gRay = GetComponent<GraphicRaycaster>();
        eSys = GetComponent<EventSystem>();
        CardCon = GetComponent<CardController>();
    }

    // Update is called once per frame
    void Update()
    {
        ped = new PointerEventData(eSys);
        ped.position = Input.mousePosition;
        List<RaycastResult> ress = new List<RaycastResult>();
        gRay.Raycast(ped, ress);

        CardCon.OnFocus(false);
        foreach (RaycastResult hit in ress)
        {
            if (hit.gameObject != null)
            {
                CardController carCon = hit.gameObject.GetComponentInParent<CardController>();
                if (carCon != null && carCon == CardCon)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        BVGameBoard.Instance.CardPlayed(CardCon);
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        CardCon.flip();
                    }
                    CardCon.OnFocus(true);
                }

                CardCon = carCon;
                LastHoverTime = Time.time;
                break;
            }
        }



    }
}
