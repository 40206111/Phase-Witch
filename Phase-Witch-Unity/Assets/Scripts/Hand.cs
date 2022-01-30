using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private static Hand _instance;
    public static Hand Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    CardController CardPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddCards(CardDatabase.GetCards(1));
        }
    }

    public void AddCards(List<CardBaseData> cards)
    {
        foreach (var card in cards)
        {
            var prefab = Instantiate(CardPrefab, transform);
            prefab.ShowCardData(card);
        }
    }
}
