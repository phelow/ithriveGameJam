using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager s_instance;

    public enum LevelStage
    {
        Day, // Talk to relatives
        Night, // Talk to ghosts
        Morning // Move the urns
    }

    [SerializeField]
    private TextMeshPro _stageText;
    private LevelStage _currentStage = LevelStage.Day;

    void Awake()
    {
        s_instance = this;
    }

    public LevelStage GetStage()
    {
        return _currentStage;
    }

    public void AdvanceState()
    {
        switch(_currentStage)
        {
            case LevelStage.Day:
                _currentStage = LevelStage.Night;
                SetSpritesToGhosts();
                break;
            case LevelStage.Night:
                SetSpritesToUrns();
                _currentStage = LevelStage.Morning;
                break;
            case LevelStage.Morning:
                CheckForEndOfGame();
                _currentStage = LevelStage.Day;
                break;
        }

        _stageText.text = _currentStage.ToString();
    }

    public void SetSpritesToGhosts()
    {
        Urn[] urns = GameObject.FindObjectsOfType<Urn>();
        foreach (Urn urn in urns)
        {
            urn.EnterGhostmode();
        }
    }

    public void SetSpritesToUrns()
    {
        Urn[] urns = GameObject.FindObjectsOfType<Urn>();
        foreach (Urn urn in urns)
        {
            urn.SetSpriteToUrn();
        }
    }

    public void CheckForEndOfGame()
    {
        Urn[] urns = GameObject.FindObjectsOfType<Urn>();
        bool victory = true;
        foreach(Urn urn in urns)
        {
            victory &= urn.VictoryCheck();
        }

        if (victory)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("win");
        }
    }

    public void OnMouseDown()
    {
        AdvanceState();
    }

}
