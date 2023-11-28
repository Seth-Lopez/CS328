using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private float waitTime = 5f;
    private float currentTime = 0f;
    private float maxDistanceFromCurrent = 5f; // Adjust this value as needed

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        MoveToRandomPosition();
    }

    void Update()
    {
        // Check if the enemy has reached the target position
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= waitTime)
            {
                currentTime = 0f;
                MoveToRandomPosition();
            }
        }
    }

    void MoveToRandomPosition()
    {
        // Generate a random position within the camera's view
        Vector3 randomPosition = new Vector3(
            Random.Range(Camera.main.rect.xMin, Camera.main.rect.xMax) * Camera.main.pixelWidth,
            Random.Range(Camera.main.rect.yMin, Camera.main.rect.yMax) * Camera.main.pixelHeight,
            0f
        );

        // Convert screen coordinates to world coordinates
        targetPosition = Camera.main.ScreenToWorldPoint(randomPosition);

        // Clamp the position within the NavMesh bounds
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 5.0f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
        }

        // Limit the distance from the enemy's current position
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, targetPosition);
        if (distance > maxDistanceFromCurrent)
        {
            Vector3 direction = (targetPosition - currentPosition).normalized;
            targetPosition = currentPosition + direction * maxDistanceFromCurrent;
        }

        // Move to the target position
        navMeshAgent.SetDestination(targetPosition);
    }
}
