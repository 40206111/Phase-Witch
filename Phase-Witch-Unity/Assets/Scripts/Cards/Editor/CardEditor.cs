using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CardEditor : EditorWindow

{

    static CardDataLists AllLists = new CardDataLists();
    static List<CardBaseData> AllCards = new List<CardBaseData>();
    static List<AbilityBaseData> AllAbilities = new List<AbilityBaseData>();
    Vector2 ScrollPos;
    bool AbilityMode = false;

    [MenuItem("Phase Witch/Card Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CardEditor window = (CardEditor)EditorWindow.GetWindow(typeof(CardEditor));
        LoadData();
        window.Show();
    }

    private void OnFocus()
    {
        LoadData();
    }

    void OnGUI()
    {
        GUILayout.Label("Card Editor", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            SaveData();
        }

        if (GUILayout.Button("Load"))
        {
            LoadData();
        }

        string cardOrAbility = AbilityMode ? "Ability" : "Card";
        if (GUILayout.Button($"New {cardOrAbility}"))
        {
            if (AbilityMode)
            {
                AbilityBaseData newAbility = new AbilityBaseData();

                if (AllAbilities == null)
                {
                    AllAbilities = new List<AbilityBaseData>();
                }
                AllAbilities.Add(newAbility);

            }
            else
            {
                CardBaseData newCard = new CardBaseData();
                if (AllCards == null)
                {
                    AllCards = new List<CardBaseData>();
                }
                AllCards.Add(newCard);
                // Auto set Card ID
                AllCards[AllCards.Count - 1].CardId = CardDataLists.NextID;
                CardDataLists.NextID++;
            }
        }

        string mode = AbilityMode ? "Switch to Cards" : "Switch to Abilities";
        if (GUILayout.Button(mode))
        {
            AbilityMode = !AbilityMode;
        }

        GUILayout.EndHorizontal();

        if (AbilityMode)
        {
            DrawAbilities();
        }
        else
        {
            DrawCards();
        }
    }

    void DrawAbilities()
    {
        if (AllLists == null)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Width(1100), GUILayout.Height(500));

        foreach (var ability in AllAbilities)
        {
            
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void DrawCards()
    {
        if (AllCards == null)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Width(1100), GUILayout.Height(500));

        foreach (var card in AllCards)
        {
            GUILayout.Label($"Card Id:  {card.CardId}");
            card.Damage = EditorGUILayout.IntField("Light Side Damage: ", card.Damage);
            card.Health = EditorGUILayout.IntField("Light Side Health: ", card.Health);

            Texture2D thingy = Resources.Load(card.CardSpritePath) as Texture2D;
            var sprite = (Texture2D)EditorGUILayout.ObjectField("Card Sprite: ", thingy, typeof(Texture2D), allowSceneObjects: true);
            if (sprite != null)
            {
                card.CardSpritePath = sprite.name;
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void SaveData()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CardData.Json");

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Debug.Log("Create Path");
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        AllLists.AllCards = AllCards;
        AllLists.AllAbilities = AllAbilities;
        
        string json = JsonConvert.SerializeObject(AllLists);
        Debug.Log($"Saved File");

        File.WriteAllText(filePath, json);
    }

    static void LoadData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "CardData.Json");

        var cardData = File.ReadAllText(filePath);
        Debug.Log(cardData);

        AllLists = JsonUtility.FromJson<CardDataLists>(cardData);
        AllCards = AllLists.AllCards;
        AllAbilities = AllLists.AllAbilities;
        CardDataLists.NextID = AllCards.Count;
        
        Debug.Log($"Loaded File");

    }

    ////this should work with webGL
    //IEnumerator ReadData()
    //{
    //    string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CardData.Json");

    //    string cardData;

    //    //find file path
    //    if (filePath.Contains("://")) //if on web
    //    {
    //        UnityWebRequest www = new UnityWebRequest(filePath);
    //        yield return www.SendWebRequest();
    //        cardData = www.downloadHandler.text;
    //    }
    //    else
    //    {
    //        cardData = System.IO.File.ReadAllText(filePath);
    //    }

    //    AllCards = JsonUtility.FromJson<List<CardBaseData>>(cardData);

    //}

}
