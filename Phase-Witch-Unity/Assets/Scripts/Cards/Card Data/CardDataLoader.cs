using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardDataLoader : MonoBehaviour
{
    public static bool DataLoaded;

    private void Start()
    {
        LoadData();
    }


    public void LoadData()
    {
        if (DataLoaded)
        {
            return;
        }

        StartCoroutine(ReadData());

        DataLoaded = true;
    }

    //this should work with webGL
    IEnumerator ReadData()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "CardData.Json");

        string data;

        //find file path
        if (filePath.Contains("://")) //if on web
        {
            UnityWebRequest www = new UnityWebRequest(filePath);
            yield return www.SendWebRequest();
            data = www.downloadHandler.text;
        }
        else
        {
            data = System.IO.File.ReadAllText(filePath);
        }

        CardDatabase.PopulateLists(JsonConvert.DeserializeObject<CardDataLists>(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }));

    }


}
