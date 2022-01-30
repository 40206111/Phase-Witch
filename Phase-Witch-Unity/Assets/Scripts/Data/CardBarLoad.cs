using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardBarLoad : MonoBehaviour
{
    [SerializeField]
    string cardBarSceneName;
    private void Awake()
    {
        SceneManager.LoadSceneAsync(cardBarSceneName, LoadSceneMode.Additive);
    }
}
