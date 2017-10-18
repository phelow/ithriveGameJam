using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour, IPointerEnterHandler
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
    private Light lightMorning;

    [SerializeField]
    //private TextMeshProUGUI _daysPassedText;

    //private int _daysPassed;

    public string nextScene;
    private int nextLevel;
    public int maxLevel = 3;

    [SerializeField]
    private SpriteRenderer talking;

    private Button _button;
    private Text _buttonText;

    private bool isWaitingForNextLevel = false;
    private bool isLoadingNextLevel = false;

    public GameObject loadingText;
    private AsyncOperation async;

    public static float ghostFadeAlpha = .35f;
    private Coroutine coroutineFlash;
    bool hasEveryoneTalked;

    private GameObject settingsButton;

    private Image fade;
    

    public void Awake() {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        talking = GameObject.Find("talking").GetComponent<SpriteRenderer>();
        _button = GameObject.Find("AdvanceState").GetComponent<Button>();
        _button.onClick.AddListener(AdvanceState);
        loadingText = GameObject.Find("Loading Text");
        loadingText.SetActive(false);
        
        _buttonText = _button.GetComponentInChildren<Text>();
        _buttonText.text = "To Night";
        s_instance = this;

        settingsButton = GameObject.Find("Settings Button");

        StartCoroutine(CheckIfCharactersTalked());
        StartLights();

        hasEveryoneTalked = false;
        fade = GameObject.Find("Fade").GetComponent<Image>();
        lightMorning = _lightMorning.GetComponent<Light>();
        
    }

    public bool IsSettingsOpen() {
        return !settingsButton.activeSelf;
    }
    

    private IEnumerator CheckIfCharactersTalked() {
        if(_currentStage == LevelStage.Morning)
        {
            yield break;
        }
        hasEveryoneTalked = false;
        
        while (!hasEveryoneTalked)
        {
            yield return new WaitForSeconds(1f);
            var characters = (_currentStage == LevelStage.Day) ? persons : ghosts;
            int count = 0;
            foreach (var c in characters)
            {
                if (c.hasTalked)
                {
                    count++;
                }
            }
            if (count == characters.Count)
            {
                hasEveryoneTalked = true;
            }
            
        }
        
        coroutineFlash = StartCoroutine(FlashButton());
    }

    private void StartLights() {
        _lightDay = GameObject.Find("#LIGHT_Day");
        _lightMorning = GameObject.Find("#LIGHT_Morning");
        _lightNight = GameObject.Find("#LIGHT_Night");
        _lightDay.SetActive(true);
        _lightMorning.SetActive(false);
        _lightNight.SetActive(false);
    }

    public void Start()
    {
        nextLevel = Convert.ToInt16(SceneManager.GetActiveScene().name.Last().ToString());
        UpdateNextLevel();

        GetCharacters();
        //persons.Remove(GameObject.Find("dad").GetComponent<Dad>());
        ShowCharacters(persons);
        HideCharacters(ghosts);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        
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
                EnableTalkForCharacters(ghosts);
                Global.soundManager.DayToNight();
                StartCoroutine(CheckIfCharactersTalked());
                break;
            case LevelStage.Night:
                _buttonText.text = "Next Day";
                DisableTalkForCharacters(ghosts);
                FadeOutGhosts();
                //HideCharacters(ghosts);
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
                
                HideCharacters(ghosts);
                ShowCharacters(persons);
                StartCoroutine(CheckIfCharactersTalked());
                break;
        }
    }

    private void FadeOutGhosts() {
       foreach(var g in ghosts)
        {
            g.Fade(ghostFadeAlpha, 10f);
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
            StartCoroutine(WinFadeEffect());    
        }
    }

    IEnumerator WinFadeEffect() {
        lightMorning = GameObject.Find("#LIGHT_Morning").GetComponent<Light>();
        yield return new WaitForSeconds(.25f);

        float tGhost = 1f;
        float tLight = 1.4f;
        float tFade = 1.5f;
        float tPassed = 0f;

        foreach (var g in ghosts)
        {
            g.Fade(1, tGhost);
        }
        
        while (tPassed < tFade + .5f)
        {
            lightMorning.intensity = Mathf.Lerp(1, 200, tPassed / tLight);
            fade.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, tPassed / tFade);
            tPassed += Time.deltaTime;
            yield return null;
        }
        
        isWaitingForNextLevel = true;
    }

    private void GetCharacters() {
        var characters = GameObject.FindObjectsOfType<Character>();
        ghosts = characters.Where(x => x.isGhost && x.transform.parent != null).ToList<Character>();
        persons = characters.Where(x => !x.isGhost && x.transform.parent != null).ToList<Character>();
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
        isWaitingForNextLevel = false;
        isLoadingNextLevel = false;
        loadingText.SetActive(false);
        StartCoroutine(FadeIn());

        if (nextLevel > maxLevel)
        {
            return;
        }
        
        GetCharacters();
        ShowCharacters(persons);
        HideCharacters(ghosts);
        StartLights();
        UpdateNextLevel();
        StartCoroutine(CheckIfCharactersTalked());

        
        
    }

    private IEnumerator FadeIn() {
        float tPassed = 0;
        float tFade = .25f;
        while(tPassed <= tFade)
        {
            fade.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), tPassed / tFade);
            tPassed += Time.deltaTime;
            yield return null;
        }
        fade.color = new Color(1, 1, 1, 0);
    }

    private void UpdateNextLevel() {
        nextLevel++;
        if (nextLevel > maxLevel)
        {
            nextScene = "Credits";
        }
        else
        {
            nextScene = "Level" + nextLevel.ToString();
        }
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
        if(nextLevel > maxLevel)
        {
            _button.gameObject.SetActive(false);
        }
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

    IEnumerator FlashButton() {
        var startColor = Color.white;
        var endColor = Color.black;
        float t = .1f;
        float tPassed = 0f;
        var image = _button.GetComponent<Image>();
        int i = 0;

        while (true)
        {
            while (tPassed < t)
            {
                image.color = Color.Lerp(startColor, endColor, tPassed / t);
                
                tPassed += Time.deltaTime;
                yield return null;
            }

            while (tPassed > 0f)
            {
                image.color = Color.Lerp(startColor, endColor, tPassed / t);
                
                tPassed -= Time.deltaTime;
                yield return null;
            }
            i++;
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(2f);
            }
        }
        
    }

    private void StopFlashButton() {
        var image = _button.GetComponent<Image>();
        image.color = Color.white;
        if (coroutineFlash != null)
        {
            StopCoroutine(coroutineFlash);
        }
    }



    private void Update() {
        if(SceneManager.GetActiveScene().name == "Credits")
        {
            return;
        }
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

    public void OnPointerEnter(PointerEventData eventData) {
        StopFlashButton();
    }
}
