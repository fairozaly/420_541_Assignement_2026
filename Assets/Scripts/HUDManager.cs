
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("HUD References")]
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text LevelText;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();

        LevelText.text =
            SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (playerStats != null)
        {
            HealthText.text =
                "HEALTH: " + playerStats.Health;
        }

        if (GameManager.Instance != null)
        {
            ScoreText.text =
                "SCORE: " + GameManager.Instance.Score;
        }
    }
}
