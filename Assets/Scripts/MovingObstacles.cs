
using UnityEngine;
using System.Collections;

public class MovingObstacles : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveHeight = 3f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Timing Settings")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float waitTime = 0.5f;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + Vector3.up * moveHeight;

        StartCoroutine(MoveObstacle());
    }

    private IEnumerator MoveObstacle()
    {
        // wait before the obstacle starts moving
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // move upward
            while (Vector3.Distance(transform.position, endPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    endPosition,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            // wait at the top
            yield return new WaitForSeconds(waitTime);

            // move downward
            while (Vector3.Distance(transform.position, startPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    startPosition,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            // wait at the bottom
            yield return new WaitForSeconds(waitTime);
        }
    }
}
