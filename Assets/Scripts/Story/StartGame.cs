using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private bool isWaitingForNextLevel = false;
    private bool isLoadingNextLevel = false;
    private string nextScene = "Level1";
    private AsyncOperation async;

    public GameObject loadingText;


    public void startButton()
    {
        
        isWaitingForNextLevel = true;
    }

    IEnumerator LoadLevel() {
        async = SceneManager.LoadSceneAsync(nextScene);
        async.allowSceneActivation = false;

        while (!isWaitingForNextLevel)
        {
            yield return null;
        }
        StopCoroutine(LoadingProgress());
        async.allowSceneActivation = true;
    }

    IEnumerator LoadingProgress() {
        var text = loadingText.GetComponent<TMPro.TextMeshProUGUI>();
        var str = "Loading... ";
        while (isWaitingForNextLevel)
        {
            str = String.Format("Loading... {0:p0}", async.progress);

            text.text = str;
            yield return null;
        }
    }

    private void Update() {
        if (!isLoadingNextLevel)
        {
            isLoadingNextLevel = true;
            StartCoroutine(LoadLevel());
        }
        if (isWaitingForNextLevel && !loadingText.activeSelf)
        {
            loadingText.SetActive(true);
            StartCoroutine(LoadingProgress());
        }
    }

}
