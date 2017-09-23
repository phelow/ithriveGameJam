using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private bool isWaitingForLevel = false;
    private bool isLoading = false;
    private string level1Name = "Level1";

    public GameObject loadingText;

    public void Awake() {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (isWaitingForLevel)
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    IEnumerator LoadLevel() {
        var async = SceneManager.LoadSceneAsync(level1Name);
        async.allowSceneActivation = false;
       
        while (!isWaitingForLevel)
        {
            yield return null;
        }
        
        async.allowSceneActivation = true;
    }

    public void startButton()
    {
        var scene = SceneManager.GetSceneByName(level1Name);
        isWaitingForLevel = true;
    }

    private void Update() {
        if (!isLoading)
        {
            StartCoroutine(LoadLevel());
            isLoading = true;
        }
        
        if(isWaitingForLevel == true)
        {
            loadingText.SetActive(true);
        }
    }

}
