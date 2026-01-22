using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapScene : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Steamworks Manager is initializing...");
        StartCoroutine(AwaitInitialization());
    }

    private IEnumerator AwaitInitialization()
    {
        Debug.Log($"Steamworks Manager status: {SteamManager.Initialized}");
        while (!SteamManager.Initialized)
        {
            yield return null;
        }
        
        Debug.Log("Steamworks Manager initialized, loading...");
        SceneManager.LoadScene("MainMenu");
    }
}