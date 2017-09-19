using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager s_instance;

    public enum LevelStage
    {
        Day, // Talk to relatives
        Night, // Talk to ghosts
        Morning // Move the urns
    }

    private LevelStage _currentStage = LevelStage.Day;

    [SerializeField]
    private List<Character> ghosts;
    [SerializeField]
    private List<Character> persons;

    [SerializeField]
    private Button _advanceButton;

    [SerializeField]
    private GameObject _lightDay;
    [SerializeField]
    private GameObject _lightNight;
    [SerializeField]
    private GameObject _lightMorning;

    [SerializeField]
    //private TextMeshProUGUI _daysPassedText;

    //private int _daysPassed;

    public string nextScene;
    private int currentLevel;

    [SerializeField]
    private SpriteRenderer talking;

    public void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(AdvanceState);
        //_music.Play("Day Loop");
        s_instance = this;
    }

    public void Start()
    {
        GetCharacters();
        persons.Remove(GameObject.Find("dad").GetComponent<Dad>());
        ShowCharacters(persons);
        HideCharacters(ghosts);
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentLevel = 1;
    }

    public void SetAdvanceButtonVisible(bool shouldBeOff)
    {
        if (_advanceButton == null)
        {
            return;
        }

        _advanceButton.interactable = !shouldBeOff;
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
                _lightDay.SetActive(false);
                _lightMorning.SetActive(false);
                _lightNight.SetActive(true);
                HideCharacters(persons);
                ShowCharacters(ghosts);
Global.soundManager.PlayMusic(Global.soundManager.nightMusic);
                break;
            case LevelStage.Night:
                HideCharacters(ghosts);
                _currentStage = LevelStage.Morning;
                _lightDay.SetActive(false);
                _lightMorning.SetActive(true);
                _lightNight.SetActive(false);
                Global.soundManager.PlayMusic(Global.soundManager.dayMusic);
                break;
            case LevelStage.Morning:
                CheckForEndOfGame();
                _currentStage = LevelStage.Day;
                _lightDay.SetActive(true);
                _lightMorning.SetActive(false);
                _lightNight.SetActive(false);
                ShowCharacters(persons);
                break;
        }
    }

    private void HideCharacters(List<Character> chars)
    {
        foreach (var c in chars)
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
        foreach (Urn urn in urns)
        {
            victory &= urn.VictoryCheck();
        }

        if (victory)
        {
            currentLevel++;
            nextScene = "Level" + currentLevel.ToString();
            SceneManager.LoadScene(nextScene);
        }
    }

    private void GetCharacters() {
        var characters = GameObject.FindObjectsOfType<Character>();
        ghosts = characters.Where(x => x.isGhost).ToList<Character>();
        persons = characters.Where(x => !x.isGhost).ToList<Character>();
    }

    public void SetTalking(Sprite sprite)
    {

        talking.sprite = sprite;
    }

    public void ClearTalking()
    {
        talking.sprite = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GetCharacters();
        ShowCharacters(persons);
        HideCharacters(ghosts);
    }
}
