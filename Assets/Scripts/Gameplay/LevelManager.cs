using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private List<Character> ghosts;
    [SerializeField]
    private List<Character> persons;

    [SerializeField]
    private Button _advanceButton;

    public string nextScene;

    private MusicManager _music;


    public void Awake()
    {
        s_instance = this;
        ShowCharacters(persons);
        HideCharacters(ghosts);
        _music = new MusicManager(gameObject);
    }

    public void SetAdvanceButtonVisible(bool shouldBeOff)
    {
        _advanceButton.interactable = !shouldBeOff;
    }

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(AdvanceState);
        //_music.Play("Day Loop");
    }

    public LevelStage GetStage()
    {
        return _currentStage;
    }

    public void AdvanceState()
    {
        switch (_currentStage)
        {
            case LevelStage.Day:
                _currentStage = LevelStage.Night;
                HideCharacters(persons);
                ShowCharacters(ghosts);
                break;
            case LevelStage.Night:
                HideCharacters(ghosts);
                _currentStage = LevelStage.Morning;
                break;
            case LevelStage.Morning:
                CheckForEndOfGame();
                _currentStage = LevelStage.Day;
                ShowCharacters(persons);
                break;
        }
        

        _stageText.text = _currentStage.ToString();
    }

    private void HideCharacters(List<Character> chars)
    {
        foreach(var c in chars)
        {
            c.gameObject.SetActive(false);
        }
    }

    private void ShowCharacters(List<Character> chars)
    {
        foreach (var c in chars)
        {
            c.gameObject.SetActive(true);
            c.ShowCharacter();
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
            SoundManager.instance._musicSource.Stop();
        }
    }

    //public void OnMouseDown()
    //{
        
    //    AdvanceState();
    //}

}
