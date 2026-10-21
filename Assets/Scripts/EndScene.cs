
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text finalScoreText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            finalScoreText.text =
                "Final Score: " + GameManager.Instance.Score;
        }
    }

    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore();
        }

        SceneManager.LoadScene("Level1");
    }
}
