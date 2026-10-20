using System.Collections;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        SpeedBoost,
        ScoreBoost,
        JumpBoost,
        Score
    }

    [Header("Pickup Settings")]
    [SerializeField] private PickupType pickupType;
    [SerializeField] private float respawnTime = 30f;

    [Header("Power Up Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float speedDuration = 5f;
    [SerializeField] private float scoreDuration = 10f;
    [SerializeField] private float jumpDuration = 30f;
    [SerializeField] private int scorePoints = 50;

    [Header("Sound")]
    [SerializeField] private AudioClip pickupSound;

    private Collider pickupCollider;
    private Renderer[] renderers;
    private bool isAvailable = true;

    private void Start()
    {
        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAvailable)
            return;

        PlayerStats stats = other.GetComponentInParent<PlayerStats>();
        CharacterMovement movement =
            other.GetComponentInParent<CharacterMovement>();

        if (stats == null || movement == null)
            return;

        // pickup effect
        switch (pickupType)
        {
            case PickupType.SpeedBoost:
                movement.ActivateSpeedBoost(
                    speedMultiplier, speedDuration);
                break;

            case PickupType.ScoreBoost:
                stats.ActivateScoreBoost(scoreDuration);
                break;

            case PickupType.JumpBoost:
                movement.ActivateTimedDoubleJump(jumpDuration);
                break;

            case PickupType.Score:
                stats.AddScore(scorePoints);
                break;
        }

        isAvailable = false;

        // play sound separately 
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(
                pickupSound, transform.position);
        }

        // score pickups are destroyed permanently
        if (pickupType == PickupType.Score)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(RespawnPickup());
        }
    }

    private IEnumerator RespawnPickup()
    {
        // disable collision
        pickupCollider.enabled = false;

        // hide pickup
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        // show pickup again
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }

        pickupCollider.enabled = true;
        isAvailable = true;
    }
}
