using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats stats =
            other.GetComponentInParent<PlayerStats>();

        if (stats != null)
        {
            stats.TakeDamage(damage);
        }
    }
}
