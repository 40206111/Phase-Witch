using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CardEditor : EditorWindow

{

    JsonSerializerSettings JsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
    static CardDataLists AllLists = new CardDataLists();
    static List<CardBaseData> AllCards = new List<CardBaseData>();
    static List<AbilityBaseData> AllAbilities = new List<AbilityBaseData>();
    Vector2 ScrollPos;
    bool AbilityMode = false;
    eAbilityType ChosenAbitlity;

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
        //LoadData();
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

        string cardOrAbility = AbilityMode ? "Ability" : "Spell Card";
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

        if (!AbilityMode)
        {
            if (GUILayout.Button($"New Unit Card"))
            {
                UnitCardData newCard = new UnitCardData();
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
            ability.AbilityType = (eAbilityType)EditorGUILayout.EnumPopup("New Ability type", ability.AbilityType);
            ability.ActivationTime = (eActivationPoint)EditorGUILayout.EnumPopup("New Ability type", ability.ActivationTime);
            ability.DarkSideData.AbilityName = EditorGUILayout.TextField("Ability Dark Name", ability.DarkSideData.AbilityName);
            ability.DarkSideData.AbilityDesc = EditorGUILayout.TextField("Ability Dark Desc", ability.DarkSideData.AbilityDesc);
            Texture2D sprite = Resources.Load(ability.DarkSideData.AbilitySpritePath) as Texture2D;
            sprite = (Texture2D)EditorGUILayout.ObjectField("Card Sprite: ", sprite, typeof(Texture2D), allowSceneObjects: true);
            //if (sprite != null)
            //{
            //    ability.DarkSideData.AbilitySpritePath = sprite.name;
            //}
            //the above wasn't working so this is my hack
            ability.DarkSideData.AbilitySpritePath = EditorGUILayout.TextField("Sprite Path", ability.DarkSideData.AbilitySpritePath);
            ability.LightSideData.AbilityName = EditorGUILayout.TextField("Ability Light Name", ability.LightSideData.AbilityName);
            ability.LightSideData.AbilityDesc = EditorGUILayout.TextField("Ability Light Desc", ability.LightSideData.AbilityDesc);
            Texture2D sprite2 = Resources.Load(ability.LightSideData.AbilitySpritePath) as Texture2D;
            sprite2 = (Texture2D)EditorGUILayout.ObjectField("Card Sprite: ", sprite2, typeof(Texture2D), allowSceneObjects: true);
            //if (sprite2 != null)
            //{
            //    ability.LightSideData.AbilitySpritePath = sprite2.name;
            //}
            //the above wasn't working so this is my hack
            ability.LightSideData.AbilitySpritePath = EditorGUILayout.TextField("Sprite Path", ability.LightSideData.AbilitySpritePath);

            if (GUILayout.Button($"Delete"))
            {
                AllAbilities.Remove(ability);
                break;
            }

            GUILayout.Space(40);
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
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Enemy Card?: ");
            card.EnemyCard = EditorGUILayout.Toggle(card.EnemyCard);
            EditorGUILayout.EndHorizontal();
            Texture2D sprite = Resources.Load(card.CardSpritePath) as Texture2D;
            sprite = (Texture2D)EditorGUILayout.ObjectField("Card Sprite: ", sprite, typeof(Texture2D), allowSceneObjects: true);
            //if (sprite != null)
            //{
            //    card.CardSpritePath = sprite.name;
            //}
            //the above wasn't working so this is my hack
            card.CardSpritePath = EditorGUILayout.TextField("Sprite Path", card.CardSpritePath);
            GUILayout.Label($"Card Id:  {card.CardId}");

            if (card is UnitCardData)
            {
                var unitCard = card as UnitCardData;

                unitCard.Damage = EditorGUILayout.IntField("Light Side Damage", unitCard.Damage);
                unitCard.Health = EditorGUILayout.IntField("Light Side Health", unitCard.Health);
            }

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);
            ChosenAbitlity = (eAbilityType)EditorGUILayout.EnumPopup("New Ability type", ChosenAbitlity);

            if (GUILayout.Button("Add Ability to card"))
            {
                Ability newAbility = AddAbilityOfType();
                if (newAbility != null)
                {
                    if (card.Abilities == null)
                    {
                        card.Abilities = new List<Ability>();
                    }
                    card.Abilities.Add(newAbility);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (card.Abilities != null)
            {
                foreach (var ability in card.Abilities)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label($"Ability type: {ability.AbilityType}");
                    EditorGUILayout.EndHorizontal();
                    ability.DarkModifier = EditorGUILayout.IntField("Dark Side Modifier", ability.DarkModifier);
                    ability.LightModifier = EditorGUILayout.IntField("Light Side Modifier", ability.LightModifier);
                    GUILayout.Space(10);

                }
            }

            if (GUILayout.Button($"Delete"))
            {
                AllCards.Remove(card);
                break;
            }

            GUILayout.Space(40);

        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    Ability AddAbilityOfType()
    {
        foreach (var ability in AllAbilities)
        {
            if (ChosenAbitlity == ability.AbilityType)
            {
                Ability newAbility = new Ability();
                newAbility.AbilityType = ChosenAbitlity;
                return newAbility;
            }
        }

        return null;
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

        string json = JsonConvert.SerializeObject(AllLists, JsonSettings);
        Debug.Log($"Saved File");

        File.WriteAllText(filePath, json);
    }

    static void LoadData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "CardData.Json");

        var cardData = File.ReadAllText(filePath);
        Debug.Log(cardData);

        AllLists = JsonConvert.DeserializeObject<CardDataLists>(cardData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        AllCards = AllLists.AllCards;
        AllAbilities = AllLists.AllAbilities;
        CardDataLists.NextID = AllCards.Count;

        Debug.Log($"Loaded File");

    }

}
