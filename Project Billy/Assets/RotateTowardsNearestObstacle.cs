using UnityEngine;

public class RotateTowardsNearestObstacle : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public string obstacleTag;
    public float rotationSpeed = 5f;

    void Update()
    {
        Transform nearestObstacle = FindNearestObstacle();

        if (nearestObstacle != null)
        {
            Vector2 directionToObstacle = nearestObstacle.position - transform.position;
            float angle = Mathf.Atan2(directionToObstacle.y, directionToObstacle.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    Transform FindNearestObstacle()
    {
        Collider2D[] obstacles;

        if (!string.IsNullOrEmpty(obstacleTag))
        {
            obstacles = Physics2D.OverlapCircleAll(transform.position, 100f, obstacleLayer);
        }
        else
        {
            obstacles = Physics2D.OverlapCircleAll(transform.position, 100f, obstacleLayer);
        }

        Transform nearestObstacle = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D obstacle in obstacles)
        {
            float distance = Vector2.Distance(transform.position, obstacle.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestObstacle = obstacle.transform;
            }
        }

        return nearestObstacle;
    }
}
