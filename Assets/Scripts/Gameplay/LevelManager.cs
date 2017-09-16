using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public enum LevelStage
    {
        Day, // Talk to relatives
        Night, // Talk to ghosts
        Morning // Move the urns
    }

    [SerializeField]
    private TextMeshPro _stageText;

    private LevelStage _currentStage = LevelStage.Day;

    void Start()
    {
    }

    public void AdvanceState()
    {
        switch(_currentStage)
        {
            case LevelStage.Day:
                _currentStage = LevelStage.Night;
                break;
            case LevelStage.Night:
                _currentStage = LevelStage.Morning;
                break;
            case LevelStage.Morning:
                CheckForEndOfGame();
                _currentStage = LevelStage.Day;
                break;
        }

        _stageText.text = _currentStage.ToString();
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
