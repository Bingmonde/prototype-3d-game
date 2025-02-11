using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private TextMeshProUGUI _time;
    private bool _isTimeRunning = true;
    float _currentTime;


    private float headDamage = 4f;
    private float libmDamage = 0.25f;
    private float BodyDamage = 1f;

    HudLogic hudLogic;
    PlayerLogic playerLogic;

    // inutilisé
    GameLevels currentLeve;

    AudioSource bgmusic;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        _time= GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        _currentTime = 0;
        StartCoroutine(TimerCoroutine());
        DisableCursor();

        hudLogic = GameObject.Find("HUD").GetComponent<HudLogic>();
        playerLogic= GameObject.Find("Player").GetComponent<PlayerLogic>();

        // identifier niveau du jeu
        currentLeve = LevelManager.instance.currentLevel;

        bgmusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        bgmusic.volume = AudioManager.instance.GetMusicVolumne();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTimeRunning) {
            DisplayTime(_currentTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("game pause");
            PauseGame();
        }
    }

    public void StopGame() {
        _isTimeRunning = false;
        StopCoroutine(TimerCoroutine());
        ActiveCursor();
    }

    // Donner le pourcentage de dégât
    public float getDamagePercentage(string bodyPart) {
        switch (bodyPart)
        {
            case "Head":
                return headDamage;
            case "Limbs":
                return libmDamage;
            case "Body":
                return BodyDamage;
            default:
                return 0f;
        }
    }


    // Mise a jour le temps par seconde
    private void DisplayTime(float currentTime)
    {
        float seconds = Mathf.FloorToInt(currentTime % 60f);
        float minutes = Mathf.FloorToInt(currentTime / 60);
        _time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private IEnumerator TimerCoroutine()
    {

        _isTimeRunning = true;
        while (_isTimeRunning)
        {
            yield return new WaitForSeconds(1f);
            _currentTime += 1f;
        }
    }


    public void ActiveCursor() {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
    }

    private void DisableCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void ReturnToMainMenu()
    {
        //        Debug.Log("return to main menu");
        MainMenuManager.HasSeenIntro = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame() {

        if (!_isTimeRunning)
        {
            return;
        }
        // arrêter le chrono
        _isTimeRunning = false;
        StopCoroutine(TimerCoroutine());
        ActiveCursor();

        // arrêter les mouvements du player et des ennemis
        playerLogic.PlayerPauseOperation(true);


        // mise à jour HUD
        hudLogic.PauseOperation(true);
    }

    public void ContinueGame() {
        // continer le chrono
        _isTimeRunning = true;
        StartCoroutine(TimerCoroutine());
        DisableCursor();

        // activer le player et les ennemie
        playerLogic.PlayerPauseOperation(false);

        // mise à jour HUD
        hudLogic.PauseOperation(false);
    }

    public bool IsGameRunning() {
        return _isTimeRunning;
    }

}
