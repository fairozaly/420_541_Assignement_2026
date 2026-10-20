using UnityEngine;

public class EndGoal : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isFinalGoal = false;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.GetComponentInParent<CharacterMovement>() == null)
            return;

        activated = true;

        if (isFinalGoal)
        {
            gameManager.LoadEndScreen();
        }
        else
        {
            gameManager.NextLevel();
        }
    }
}
