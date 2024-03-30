using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public float distanceFromParent = 2f; // Set distance from the parent object
    public float moveSpeed = 2f;

    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
    }

    void FixedUpdate()
    {
        // Calculate the target position at a set distance from the parent
        Vector2 targetPosition = (Vector2)parentTransform.position + (Vector2)(transform.position - parentTransform.position).normalized * distanceFromParent;

        // Find the closest obstacle
        Collider2D closestObstacle = FindClosestObstacle(targetPosition);

        if (closestObstacle != null)
        {
            // Move around the obstacle
            CircleAroundObstacle(closestObstacle.transform.position);
        }
        else
        {
            // Move towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    Collider2D FindClosestObstacle(Vector2 targetPosition)
    {
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(targetPosition, 0.1f, obstacleLayer); // Adjust the radius as needed
        Collider2D closestObstacle = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D obstacle in obstacles)
        {
            float distance = Vector2.Distance(targetPosition, obstacle.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObstacle = obstacle;
            }
        }

        return closestObstacle;
    }

    void CircleAroundObstacle(Vector3 obstaclePosition)
    {
        // Calculate direction from enemy to obstacle
        Vector3 toObstacle = obstaclePosition - parentTransform.position;

        // Calculate perpendicular direction to circle around the obstacle
        Vector3 circleDirection = Vector3.Cross(toObstacle, Vector3.forward).normalized;

        // Calculate the point on the circle's perimeter
        Vector3 circlePerimeter = obstaclePosition + circleDirection * distanceFromParent;

        // Move towards the point on the circle's perimeter
        transform.position = Vector3.MoveTowards(transform.position, circlePerimeter, moveSpeed * Time.deltaTime);
    }
}
