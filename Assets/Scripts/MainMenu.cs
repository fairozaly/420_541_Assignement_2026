
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Reset score before starting
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();
        }

        SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
