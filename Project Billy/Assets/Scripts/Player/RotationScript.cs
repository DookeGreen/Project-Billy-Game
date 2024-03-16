using System.Collections;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] private float rotationAmount; // Rotation amount in degrees
    [SerializeField] private float rotationTime; // Time to complete rotation in seconds

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StopAllCoroutines();
            StartCoroutine(RotateOverTime());
        }
    }

    IEnumerator RotateOverTime()
    {
        for(int i = 0; i < 4; i++)
        {
            // Store the initial rotation of the GameObject
            Quaternion startRotation = transform.rotation;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, startRotation.eulerAngles.z + rotationAmount);

            // Time elapsed during rotation
            float timeElapsed = 0f;

            // Perform rotation over time
            while (timeElapsed < rotationTime)
            {
                // Calculate the interpolation factor based on time
                float t = timeElapsed / rotationTime;

                // Interpolate the rotation between the start and target rotations
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

                // Increment time elapsed
                timeElapsed += Time.deltaTime;

                // Wait for the next frame
                yield return null;
            }

            // Ensure the rotation ends exactly at the target rotation
            transform.rotation = targetRotation;
        }
    }
}
