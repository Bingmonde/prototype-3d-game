using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;
//using static System.Net.Mime.MediaTypeNames;


public class MainMenuManager : MonoBehaviour
{
    public static bool HasSeenIntro = false;


    [SerializeField] private float animationDuration1 = 1.0f;
    [SerializeField] private float animationDuration2 = 2f;
    [SerializeField] private float animationDuration1_5 = 1.5f;
    [SerializeField] private float transitionInterval = 0.5f;
    [SerializeField] private float startButtonDuration = 1f;


    private TextMeshProUGUI titleGame;
    private Transform positionTitleGame;
 
    private Button buttonStart;
    private TextMeshProUGUI textStart;
    private TextMeshProUGUI startShortcut;

    private Button buttonPlay;
    //private Transform positionButtonPlay;
    private TextMeshProUGUI playShortcut;
    private TextMeshProUGUI textPlay;

    private Button buttonOptions;
    private TextMeshProUGUI textOptions;
    private TextMeshProUGUI optionsShortcut;

    private TextMeshProUGUI titlePlay;
    private Button buttonNormal;
    private TextMeshProUGUI textNormal;
    private Button buttonDifficult;
    private TextMeshProUGUI textDifficult;


    private TextMeshProUGUI titleOptions;
    private Button buttonReturn;
    private TextMeshProUGUI textReturn;
    private TextMeshProUGUI returnShorcut;

    private TextMeshProUGUI textMusic;
    private TextMeshProUGUI musicVolumn;
    private TextMeshProUGUI textSound;
    private TextMeshProUGUI soundVolumn;

    private GameObject soundSlider;
    private GameObject musicSlide;
    private GameObject buttonReturnSlider;


    //private CanvasGroup soundSlider;

    private Image characterImage;
    private GameObject positionCharacter;

    private Vector3 slideOffset;
    private Vector3 titleOffset;

    private Color frontColor;
    private Color backColor;

    private Color fontColorTransparent;
    private Color backColorTransparent;

    private float transparentAlpha = 0;
    private float fullAlpha = 1;

    AudioSource musicSource;

    private MenuStat currentMenu = MenuStat.MenuStat_Welcome;
      private enum MenuStat
        {
            MenuStat_Welcome,
            MenuStat_Main,
            MenuStat_Options,
            MenuStat_Play,
        }


    void Start()
    {

        titleGame = GameObject.Find("TitleGame").GetComponent<TextMeshProUGUI>();
        positionTitleGame = GameObject.Find("PositionTitleGame").GetComponent<Transform>();

        buttonStart = GameObject.Find("ButtonStart").GetComponent<Button>();
        textStart = GameObject.Find("Start").GetComponent <TextMeshProUGUI>();
        startShortcut = GameObject.Find("StartShortcut").GetComponent<TextMeshProUGUI>();

        buttonPlay = GameObject.Find("ButtonPlay").GetComponent<Button>();
        //positionButtonPlay = GameObject.Find("PositionButtonPlay").GetComponent<Transform>();
        textPlay = GameObject.Find("Play").GetComponent<TextMeshProUGUI>();
        playShortcut = GameObject.Find("PlayShortcut").GetComponent<TextMeshProUGUI>();

        buttonOptions = GameObject.Find("ButtonOptions").GetComponent<Button>();
        textOptions = GameObject.Find("Options").GetComponent<TextMeshProUGUI>();
        optionsShortcut = GameObject.Find("OptionsShortcut").GetComponent<TextMeshProUGUI>();

        titlePlay = GameObject.Find("TitlePlay").GetComponent<TextMeshProUGUI>();
        textNormal = GameObject.Find("Normal").GetComponent<TextMeshProUGUI>();
        textDifficult = GameObject.Find("Difficult").GetComponent<TextMeshProUGUI>();
        buttonNormal = GameObject.Find("ButtonNormal").GetComponent<Button>();
        buttonDifficult = GameObject.Find("ButtonDifficult").GetComponent<Button>();

        titleOptions = GameObject.Find("TitleOptions").GetComponent<TextMeshProUGUI>();

        buttonReturn = GameObject.Find("ButtonReturn").GetComponent<Button>();
        
        textReturn = GameObject.Find("Return").GetComponent<TextMeshProUGUI>();
        returnShorcut = GameObject.Find("ReturnShortcut").GetComponent<TextMeshProUGUI>();

        textMusic = GameObject.Find("MusicText").GetComponent<TextMeshProUGUI>();
        musicVolumn = GameObject.Find("MusicVolumn").GetComponent<TextMeshProUGUI>();
        textSound = GameObject.Find("SoundText").GetComponent<TextMeshProUGUI>();
        soundVolumn = GameObject.Find("SoundVolumn").GetComponent<TextMeshProUGUI>();
        /*soundSlider = GameObject.Find("SoundBar").GetComponent<Slider>();
        musicSlide = GameObject.Find("MusicBar").GetComponent<Slider>();*/
        soundSlider = GameObject.Find("SoundBar");
        musicSlide = GameObject.Find("MusicBar");
        buttonReturnSlider = GameObject.Find("ButtonReturnSlider");

        characterImage = GameObject.Find("Character").GetComponent<Image>();
        positionCharacter = GameObject.Find("PositionCharacter");


        slideOffset = new Vector3(80, 0, 0);
        titleOffset = positionTitleGame.position - titleGame.transform.position;

        Debug.Log(slideOffset);

        backColor = buttonStart.GetComponent<Image>().color;
        frontColor = textPlay.color;
        backColor = new Color(backColor.r, backColor.g, backColor.b, 1);
        frontColor = new Color(frontColor.r, frontColor.g, frontColor.b, 1);
        backColorTransparent = new Color(backColor.r, backColor.g, backColor.b, 0);
        fontColorTransparent = new Color(frontColor.r, frontColor.g, frontColor.b, 0);


        // désactiver les boutons sauf COMMENCER
        buttonDifficult.enabled = false;
        buttonNormal.enabled = false;
        buttonPlay.enabled = false;
        buttonOptions.enabled = false;

        buttonDifficult.interactable = false;
        buttonNormal.interactable = false;
        buttonPlay.interactable = false;
        buttonOptions.interactable = false;
        buttonReturn.interactable=false;
        buttonReturnSlider.SetActive(false);

        //soundSlider.interactable = false;

        soundSlider.SetActive(false);
        musicSlide.SetActive(false);

        musicSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        //OnMusicSliderChange();
        // OnSoundSliderChange();

        if (MainMenuManager.HasSeenIntro) {
            EnterMainMenuFromOtherMenu(0f);
        }
        else
        {
            ToMenuTitre();
        }

    }


    void Update()
    {
        // raccourci
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentMenu = MenuStat.MenuStat_Main;
            MenuTitreToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentMenu = MenuStat.MenuStat_Play;
            MainMenutoMenuPlay();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            currentMenu = MenuStat.MenuStat_Options;
            MainMenuToMenuOption();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (currentMenu == MenuStat.MenuStat_Options)
                MenuOptionToMainMenu();
            if (currentMenu == MenuStat.MenuStat_Play)
                MenuPlayBackToMenuPrincipal();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToNomalLevelGame();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GoToDiffiLevelGame();
        }


    }


    public void GoToNomalLevelGame() {
        LevelManager.instance.SetCurrentGameLevel(GameLevels.Normal);
        SceneManager.LoadScene("Level_01");
    }

    public void GoToDiffiLevelGame()
    {
        LevelManager.instance.SetCurrentGameLevel(GameLevels.Difficle);
        SceneManager.LoadScene("Level_01");
    }




    // ------------------------ menu titre -> menu principal ----------------------------
    public void MenuTitreToMainMenu() {
        buttonStart.interactable = false;
        buttonPlay.interactable = true;
        buttonOptions.interactable = true;

        // Titre du jeu monte, 1.5s
        StartCoroutine(ElementSlide(titleGame.transform, titleGame.transform.position + titleOffset, animationDuration1_5, 0));

        // Button COMMENCER fade-out et slide-out, 1.5s
        StartCoroutine(TextFade(textStart, transparentAlpha, animationDuration1_5, 0));
        StartCoroutine(TextFade(startShortcut, transparentAlpha, animationDuration1_5, 0));
        StartCoroutine(ButtonFade(buttonStart, transparentAlpha, animationDuration1_5, 0));
        StartCoroutine(ElementSlide(buttonStart.transform, buttonStart.transform.position - slideOffset, animationDuration1_5, 0));

        float fadeTime = animationDuration1_5;
        // Button JOUER slide-in (gauche -> droit) et fade-in, 1.5s
        StartCoroutine(TextFade(textPlay,  fullAlpha, animationDuration1_5, fadeTime));
        StartCoroutine(TextFade(playShortcut,  fullAlpha, animationDuration1_5, fadeTime));
        StartCoroutine(ButtonFade(buttonPlay, fullAlpha, animationDuration1_5, fadeTime));
        StartCoroutine(ElementSlide(buttonPlay.transform, buttonPlay.transform.position + slideOffset, animationDuration1_5, fadeTime));

        // Button JOUER slide-in (gauche -> droit) et fade-in, 1.5s, intervalle 0.5s
        StartCoroutine(ElementSlide(buttonOptions.transform,
                        buttonOptions.transform.position + slideOffset,
                        animationDuration1_5, transitionInterval + fadeTime));
        StartCoroutine(TextFade(textOptions, fullAlpha, animationDuration1_5 + fadeTime, transitionInterval + fadeTime));
        StartCoroutine(TextFade(optionsShortcut, fullAlpha, animationDuration1_5 + fadeTime, transitionInterval + fadeTime));
        StartCoroutine(ButtonFade(buttonOptions, fullAlpha, animationDuration1_5 + fadeTime, transitionInterval + fadeTime));

        
    }

    // ------------------------ menu principal -> menu jouer ----------------------------

    public void MainMenutoMenuPlay() {
        float fadeoutTime = QuitMainMenu(0f);
        EnterMenuPlay(fadeoutTime);
    }


    // ------------------------ menu jouer -> menu principal ----------------------------
    public void MenuPlayBackToMenuPrincipal() 
    {
        float waitingTime = QuitMenuPlay(0f);
        EnterMainMenuFromOtherMenu(waitingTime);
        Debug.Log(buttonPlay.GetComponent<Image>().color);
    }

    // ------------------------ menu principal -> menu options ----------------------------
    public void MainMenuToMenuOption()
    {
        float waitingTime = QuitMainMenu(0f);
        EnterMenuOptions(waitingTime);
    }

    // ------------------------ menu options -> menu principal ----------------------------
    public void MenuOptionToMainMenu() {
        float waitingTime = QuitMenuOptions(0f);
        EnterMainMenuFromOtherMenu(waitingTime);
    }



    // ------------------------enter menu titre ----------------------------
    private void ToMenuTitre()
    {
        // fade-in du titre, personnage slide-in, 2s
        StartCoroutine(TextFade(titleGame, fullAlpha, animationDuration2, 0));

        float waitTime = animationDuration2;
        // fade-in du button COMMENCER, 2s, apres fade-in u
        StartCoroutine(TextFade(textStart, fullAlpha, animationDuration2, waitTime));
        StartCoroutine(TextFade(startShortcut, fullAlpha, animationDuration2, waitTime));
        StartCoroutine(ButtonFade(buttonStart, fullAlpha, animationDuration2, waitTime));

        // slide-in du image
        StartCoroutine(ElementSlide(characterImage.transform, positionCharacter.transform.position, animationDuration2, waitTime));
    }

    private float QuitMainMenu(float waitingTime)
    {
        buttonPlay.interactable = false;
        buttonOptions.interactable = false;
        // fade-out titre du jeu
        StartCoroutine(TextFade(titleGame, transparentAlpha, animationDuration1_5, waitingTime));

        // slide-out et fade-out du bouton JOUER 
        StartCoroutine(TextFade(textPlay, transparentAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(TextFade(playShortcut, transparentAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ButtonFade(buttonPlay, transparentAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ElementSlide(buttonPlay.transform, buttonPlay.transform.position - slideOffset, animationDuration1_5, waitingTime));

        // slide-out et fade-out du bouton OPTIONS  
        StartCoroutine(TextFade(textOptions, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(TextFade(optionsShortcut, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ButtonFade(buttonOptions, transparentAlpha, animationDuration1_5, waitingTime +transitionInterval));
        StartCoroutine(ElementSlide(buttonOptions.transform, buttonOptions.transform.position - slideOffset, animationDuration1_5, waitingTime + transitionInterval));

        return waitingTime + animationDuration1_5 + transitionInterval;
    }

    private float EnterMainMenuFromOtherMenu(float waitingTime) {
        buttonPlay.interactable = true;
        buttonOptions.interactable = true;

        // fade-in du titre du jeu
        StartCoroutine(TextFade(titleGame, fullAlpha, animationDuration1_5, waitingTime));

        // slide-in et fade-in du bouton JOUER 
        StartCoroutine(TextFade(textPlay, fullAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(TextFade(playShortcut, fullAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ButtonFade(buttonPlay, fullAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ElementSlide(buttonPlay.transform, buttonPlay.transform.position + slideOffset, animationDuration1_5, waitingTime));

        /*        StartCoroutine(TextFade(textOptions, fullAlpha, animationDuration1_5, waitingTime));
                StartCoroutine(TextFade(optionsShortcut, fullAlpha, animationDuration1_5, waitingTime));
                StartCoroutine(ButtonFade(buttonOptions, fullAlpha, animationDuration1_5, waitingTime ));
                StartCoroutine(ElementSlide(buttonOptions.transform, buttonOptions.transform.position + slideOffset, animationDuration1_5, waitingTime ));*/

        // slide-in et fade-in du bouton OPTIONS  
        StartCoroutine(TextFade(textOptions, fullAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(TextFade(optionsShortcut, fullAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ButtonFade(buttonOptions, fullAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ElementSlide(buttonOptions.transform, buttonOptions.transform.position + slideOffset, animationDuration1_5, waitingTime + transitionInterval));

        return animationDuration1_5 + waitingTime + transitionInterval;


    }
    private float EnterMenuPlay(float waitingTime) {

        // fade-in titre play
        StartCoroutine(TextFade(titlePlay, fullAlpha, animationDuration1_5, waitingTime));

        // slide-in et fade-in du bouton NORMAL
        StartCoroutine(TextFade(textNormal, fullAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ButtonFade(buttonNormal, fullAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ElementSlide(buttonNormal.transform, buttonNormal.transform.position + slideOffset, animationDuration1_5, waitingTime));

        // slide-in et fade-in du bouton DIFFICILE
        StartCoroutine(TextFade(textDifficult, fullAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ButtonFade(buttonDifficult, fullAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ElementSlide(buttonDifficult.transform, buttonDifficult.transform.position + slideOffset, animationDuration1_5, waitingTime + transitionInterval));

        // slide-in et fade-in du bouton RETOUR
        StartCoroutine(TextFade(textReturn, fullAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(TextFade(returnShorcut, fullAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(ButtonFade(buttonReturn, fullAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(ElementSlide(buttonReturn.transform, buttonReturn.transform.position + slideOffset, animationDuration1_5, waitingTime + transitionInterval * 2));

        buttonDifficult.interactable = true;
        buttonNormal.interactable = true;
        buttonReturn.interactable = true;

        return waitingTime + animationDuration1_5 + transitionInterval * 2;

    }

    private float QuitMenuPlay(float waitingTime) {
        //  fade-out titre play
        StartCoroutine(TextFade(titlePlay, transparentAlpha, animationDuration1_5, waitingTime));

        // slide-out et fade-out du bouton NORMAL
        StartCoroutine(TextFade(textNormal, transparentAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ButtonFade(buttonNormal, transparentAlpha, animationDuration1_5, waitingTime));
        StartCoroutine(ElementSlide(buttonNormal.transform, buttonNormal.transform.position - slideOffset, animationDuration1_5, waitingTime));

        // slide-out et fade-out du bouton DIFFICILE
        StartCoroutine(TextFade(textDifficult, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ButtonFade(buttonDifficult, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval));
        StartCoroutine(ElementSlide(buttonDifficult.transform, buttonDifficult.transform.position - slideOffset, animationDuration1_5, waitingTime + transitionInterval));

        // slide-out et fade-out du bouton RETOUR
        StartCoroutine(TextFade(textReturn, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(TextFade(returnShorcut, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(ButtonFade(buttonReturn, transparentAlpha, animationDuration1_5, waitingTime + transitionInterval * 2));
        StartCoroutine(ElementSlide(buttonReturn.transform, buttonReturn.transform.position - slideOffset, animationDuration1_5, waitingTime + transitionInterval * 2));

        buttonDifficult.interactable = false;
        buttonNormal.interactable = false;
        buttonReturn.interactable = false;

        return waitingTime + animationDuration1_5 + transitionInterval * 2;
    }

    private float EnterMenuOptions(float waitingTime) {

        // fade-in du titre options
        StartCoroutine(TextFade(titleOptions, fullAlpha, animationDuration1, waitingTime));

        // fade-in texts sons, 1s, 
        StartCoroutine(TextFade(textSound, fullAlpha, animationDuration1, waitingTime));
        StartCoroutine(TextFade(soundVolumn, fullAlpha, animationDuration1, waitingTime));
        //StartCoroutine(SlideFade(soundSlider, fullAlpha, animationDuration1, waitingTime));

        // fade-in texts musiques, 1s, interval 1s
        StartCoroutine(TextFade(textMusic, fullAlpha, animationDuration1, waitingTime + animationDuration1));
        StartCoroutine(TextFade(musicVolumn, fullAlpha, animationDuration1, waitingTime + animationDuration1));

        soundSlider.SetActive(true);
        musicSlide.SetActive(true);
        buttonReturnSlider.SetActive(true);

        return waitingTime + animationDuration1 *2;
    }

    private float QuitMenuOptions(float waitingTime) {
        buttonOptions.interactable = true;
        // fade-out du titre options
        StartCoroutine(TextFade(titleOptions, transparentAlpha, animationDuration1, waitingTime));

        // fade-out texts sons, 1s, 
        StartCoroutine(TextFade(textSound, transparentAlpha, animationDuration1, waitingTime));
        StartCoroutine(TextFade(soundVolumn, transparentAlpha, animationDuration1, waitingTime));
        //StartCoroutine(SlideFade(soundSlider, transparentAlpha, animationDuration1, waitingTime));

        // fade-out texts musiques, 1s, interval 1s
        StartCoroutine(TextFade(textMusic, transparentAlpha, animationDuration1, waitingTime + animationDuration1));
        StartCoroutine(TextFade(musicVolumn, transparentAlpha, animationDuration1, waitingTime + animationDuration1));

        soundSlider.SetActive(false);
        musicSlide.SetActive(false);
        buttonReturnSlider.SetActive(false);
        return waitingTime + animationDuration1 * 2;
    }


    // fade-in / fade-out
    private IEnumerator TextFade(TextMeshProUGUI text, float endAlpha, float duration, float waitingTime)
    {
        if (waitingTime != 0) {
            yield return new WaitForSeconds(waitingTime);
        }

        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {

            float currentTime = t / duration;
            text.color = Color.Lerp(startColor, endColor, currentTime);
            yield return null;
        }
        
    }

    private IEnumerator ButtonFade(Button button,  float endAlpha, float duration, float waitingTime)
    {
        if (waitingTime != 0)
        {
            yield return new WaitForSeconds(waitingTime);
        }

        Color startColor = button.GetComponent<Image>().color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {

            float currentTime = t / duration;

            button.GetComponent<Image>().color = Color.Lerp(startColor, endColor, currentTime);
            
            yield return null;
        }
    }

    private IEnumerator SlideFade(CanvasGroup group, float changeAlpha, float duration, float waitingTime)
    {
        if (waitingTime != 0)
        {
            yield return new WaitForSeconds(waitingTime);
        }

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {

            group.alpha += changeAlpha / duration;
           // group.alpha = Mathf.Clamp01((float)t / animationDuration1_5);
        }



    }

    private IEnumerator ElementSlide(Transform elementTrans, Vector3 posEnd, float duration, float waitingTime)
    {
        if (waitingTime != 0)
        {
            yield return new WaitForSeconds(waitingTime);
        }

        Vector3 posStart = elementTrans.position;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            elementTrans.position = Vector3.Lerp(posStart, posEnd, t / duration);
            yield return 0;
        }
    }

    public void OnMusicSliderChange()
    {
        musicSlide.SetActive(true);
        musicVolumn.text = Mathf.CeilToInt(musicSlide.GetComponent<Slider>().value * 100).ToString();
        Debug.Log(musicVolumn.text);
        musicSource.volume = musicSlide.GetComponent<Slider>().value;
        AudioManager.instance.SetMusicVolumne(musicSlide.GetComponent<Slider>().value);
    }

    public void OnSoundSliderChange()
    {
        soundSlider.SetActive(true);
        soundVolumn.text = Mathf.CeilToInt(soundSlider.GetComponent<Slider>().value * 100).ToString();
        AudioManager.instance.SetSoundVolumne(soundSlider.GetComponent<Slider>().value);
    }

}
