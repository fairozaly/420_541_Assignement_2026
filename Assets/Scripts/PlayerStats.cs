using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 3;

    [Header("Damage Settings")]
    [SerializeField] private float damageCooldown = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private bool scoreBoostActive = false;
    private Coroutine scoreBoostCoroutine;

    public int Health => currentHealth;
    public int Score => GameManager.Instance.Score;
    public bool ScoreBoostActive => scoreBoostActive;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // add points, (taking Score Boost into account)
    public void AddScore(int points)
    {
        int multiplier = scoreBoostActive ? 2 : 1;

        GameManager.Instance.AddScore(points * multiplier);
    }

    // take damage from a trap
    public void TakeDamage(int damage)
    {
        if (!canTakeDamage || currentHealth <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    // enable double score for a limited duration
    public void ActivateScoreBoost(float duration)
    {
        if (scoreBoostCoroutine != null)
            StopCoroutine(scoreBoostCoroutine);

        scoreBoostCoroutine =
            StartCoroutine(ScoreBoostTimer(duration));
    }

    private IEnumerator ScoreBoostTimer(float duration)
    {
        scoreBoostActive = true;

        yield return new WaitForSeconds(duration);

        scoreBoostActive = false;
        scoreBoostCoroutine = null;
    }
}
