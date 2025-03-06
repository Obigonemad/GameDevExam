using System.Collections;
using UnityEngine;

public class PlatformVisibilityManager : MonoBehaviour
{
    // Public variables to set in the Inspector for random ranges
    public float minVisibleTime = 3f;   // Minimum time the platform is visible
    public float maxVisibleTime = 7f;   // Maximum time the platform is visible
    public float minCycleTime = 6f;     // Minimum total cycle time
    public float maxCycleTime = 12f;    // Maximum total cycle time

    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect
    public float shakeDuration = 2f;    // Duration for the shake effect

    private Renderer platformRenderer;  // The renderer to toggle visibility
    private Collider platformCollider;  // The collider to toggle interaction
    private Vector3 originalPosition;   // Original position of the platform

    private void Start()
    {
        // Get the Renderer and Collider components of the platform
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();

        // Save the original position of the platform
        originalPosition = transform.position;

        // Start the visibility toggle coroutine with random timers
        StartCoroutine(PlatformVisibilityCycle());
    }

    private IEnumerator PlatformVisibilityCycle()
    {
        // Loop indefinitely to repeat the cycle
        while (true)
        {
            // Generate random values for visibleTime and totalCycleTime
            float visibleTime = Random.Range(minVisibleTime, maxVisibleTime);
            float totalCycleTime = Random.Range(minCycleTime, maxCycleTime);
            float invisibleTime = totalCycleTime - visibleTime; // Remaining time is invisible

            // Make the platform visible (enable the Renderer and Collider)
            platformRenderer.enabled = true;
            platformCollider.enabled = true;

            // Wait for the time the platform should be visible
            yield return new WaitForSeconds(visibleTime);

            // Shake the platform for 2 seconds before it becomes invisible
            yield return StartCoroutine(ShakePlatform(shakeDuration));

            // Make the platform invisible (disable the Renderer and Collider)
            platformRenderer.enabled = false;
            platformCollider.enabled = false;

            // Wait for the remaining time in the cycle
            yield return new WaitForSeconds(invisibleTime);
        }
    }

    private IEnumerator ShakePlatform(float duration)
    {
        float elapsedTime = 0f;

        // Shake the platform for the given duration
        while (elapsedTime < duration)
        {
            // Randomize the shake direction and magnitude
            Vector3 randomShake = new Vector3(
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude),
                Random.Range(-shakeMagnitude, shakeMagnitude)
            );

            // Apply the shake to the platform's position
            transform.position = originalPosition + randomShake;

            // Wait a small time before shaking again
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return the platform to its original position after shaking
        transform.position = originalPosition;
    }
}
