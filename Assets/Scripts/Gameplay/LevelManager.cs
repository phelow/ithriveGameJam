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
    public int maxLevel = 3;

    [SerializeField]
    private SpriteRenderer talking;

    private Button _button;
    private Text _buttonText;

    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(AdvanceState);
        _buttonText = _button.GetComponentInChildren<Text>();
        _buttonText.text = "To Night";
        s_instance = this;

        Global.soundManager.PlayMusic();
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
                _buttonText.text = "Move Urns";
                _currentStage = LevelStage.Night;
                _lightDay.SetActive(false);
                _lightMorning.SetActive(false);
                _lightNight.SetActive(true);
                HideCharacters(persons);
                ShowCharacters(ghosts);
                //EnableTalkForCharacters(ghosts);
                Global.soundManager.DayToNight();
                break;
            case LevelStage.Night:
                _buttonText.text = "Next Day";
                //DisableTalkForCharacters(ghosts);
                HideCharacters(ghosts);
                _currentStage = LevelStage.Morning;
                _lightDay.SetActive(false);
                _lightMorning.SetActive(true);
                _lightNight.SetActive(false);
                Global.soundManager.NightToDay();
                break;
            case LevelStage.Morning:
                _buttonText.text = "To Night";
                _currentStage = LevelStage.Day;
                _lightDay.SetActive(true);
                _lightMorning.SetActive(false);
                _lightNight.SetActive(false);
                if (CheckForEndOfGame())
                {
                    return;
                }
                
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

    private void DisableTalkForCharacters(List<Character> chars) {
        foreach(var c in chars)
        {
            c.DisableTalk();
        }
    }

    private void EnableTalkForCharacters(List<Character> chars) {
        foreach (var c in chars)
        {
            c.EnableTalk();
        }
    }


    public bool CheckForEndOfGame()
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
            if (currentLevel > maxLevel)
            {
                nextScene = "Win";
            }
            else
            {
                nextScene = "Level" + currentLevel.ToString();
            }
            
            
            SceneManager.LoadScene(nextScene);
            return true;
        }
        return false;
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
