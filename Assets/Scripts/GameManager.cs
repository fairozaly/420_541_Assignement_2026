using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager Instance { get; private set; }
    [SerializeField] private string endScene = "EndScene";

    [Header("Game Stats")]
    [SerializeField] private int score = 0;

    public int Score => score;

    private void Awake()
    {
        // only one Game Manager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // keep manager when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    // add points to the total score
    public void AddScore(int points)
    {
        score += points;

        Debug.Log("Total Score: " + score);
    }

    public void NextLevel()
    {
        int nextLevel =
            SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            LoadEndScreen();
        }
    }

    // resst score when starting a new game
    public void ResetScore()
    {
        score = 0;
    }

        
    public void LoadEndScreen()
    {
        SceneManager.LoadScene(endScene);
    }
}