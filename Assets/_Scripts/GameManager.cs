using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAMEOVER , GS_OPTIONS};
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState currentState = GameState.GS_PAUSEMENU;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public TMP_Text scoreText;
    public TMP_Text qualityText;
    public TMP_Text levelCompletedScoreText;
    public TMP_Text levelCompletedHighScoreText;
    public int score = 0;
    public TMP_Text enemiesText;
    public int enemiesKilled = 0;
    public int keysFound = 0;
    public Image[] keysTab;
    public Image[] heartsTab;
    public int lives = 3;

    [SerializeField] private TMP_Text timeText;
    private float timer = 0;

    private const string mainMenuLevelName = "mainMenu";
    private const string keyHighScore = "HighScoreLevel1";

    private const string level1name = "level1";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        scoreText.text = score.ToString();
        enemiesText.text = enemiesKilled.ToString();

        if (!PlayerPrefs.HasKey(keyHighScore))
        {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }

        foreach (var img in keysTab)
        {
            img.color = Color.gray;
        }


        InGame();
    }

    private void Update()
    {
        if (currentState == GameState.GS_GAME)
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.GS_PAUSEMENU)
                InGame();
            else
                PauseMenu();
        }

        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.Floor(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void AddLife(int lifeNumber)
    {
        heartsTab[lifeNumber].enabled = true;
        lives++;
    }
    public void DeleteLife()
    {
        lives--;

        heartsTab[lives].enabled = false;
    }
    public void AddKeys(int keyNumber)
    {
        keysTab[keyNumber].color = Color.red;
        keysFound++;
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void AddKilledEnemy()
    {
        enemiesKilled++;
        enemiesText.text = enemiesKilled.ToString();
    }
    public void SetGameState(GameState newState)
    {
        if (newState == currentState)
            return;

        currentState = newState;
        if (currentState == GameState.GS_PAUSEMENU)
        {
            pauseMenuCanvas.enabled = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuCanvas.enabled = false;
            Time.timeScale = 1f;
        }
        if (currentState == GameState.GS_GAME)
            inGameCanvas.enabled = true;
        else
            inGameCanvas.enabled = false;

        if (currentState == GameState.GS_LEVELCOMPLETED)
        {
            levelCompletedCanvas.enabled = true;
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == level1name)
            {
                int highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }
                levelCompletedHighScoreText.text = "High score: " + highScore;
                levelCompletedScoreText.text = "Score: " + score;
            }
        }
        else
            levelCompletedCanvas.enabled = false;

        if (currentState == GameState.GS_OPTIONS)
        {
            optionsCanvas.enabled = true;
            Time.timeScale = 0f;
        }
        else
        {
            optionsCanvas.enabled = false;
            Time.timeScale = 1f;
        }
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }
    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    public void GameOver()
    {
        SetGameState(GameState.GS_GAMEOVER);
    }
    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene(mainMenuLevelName);
    }

    public void GraphicIncrease()
    {
        QualitySettings.IncreaseLevel();
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void GraphicDecrease()
    {
        QualitySettings.DecreaseLevel(); 
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void SetVolume(float volume) => AudioListener.volume = volume;
   
}
