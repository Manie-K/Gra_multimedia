using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAMEOVER };
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState currentState = GameState.GS_PAUSEMENU;
    public Canvas inGameCanvas;
    public TMP_Text scoreText;
    public int score = 0;
    public TMP_Text enemiesText;
    public int enemiesKilled = 0;
    public int keysFound = 0;
    public Image[] keysTab;
    public Image[] heartsTab;
    public int lives = 3;

    [SerializeField] private TMP_Text timeText;
    private float timer = 0;


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

        foreach (var img in keysTab)
        {
            img.color = Color.gray;
        }
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
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
        inGameCanvas.enabled = false;
    }
    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        inGameCanvas.enabled = true;
    }
    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
        inGameCanvas.enabled = false;
    }
    public void GameOver()
    {
        SetGameState(GameState.GS_GAMEOVER);
        inGameCanvas.enabled = false;
    }
}
