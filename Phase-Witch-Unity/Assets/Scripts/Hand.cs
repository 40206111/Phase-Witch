using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    CardController CardPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var cards = CardDatabase.GetCards(1);
            foreach (var card in cards)
            {
                var prefab = Instantiate(CardPrefab, transform);
                prefab.ShowCardData(card);
            }
        }
    }


}
