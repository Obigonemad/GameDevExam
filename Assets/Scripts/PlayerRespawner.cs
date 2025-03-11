using System.Collections;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    private Vector3 respawnPosition;
    public float respawnSpeed = 2.0f; // Speed of transition
    public float duration = 1.5f; // Duration of the transition

    private void Start()
    {
        respawnPosition = transform.position; // Start with the initial spawn point
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        respawnPosition = checkpointPosition;
        Debug.Log($"PlayerRespawner checkpoint position updated to: {respawnPosition}");
    }

    // Ny offentlig metode til at starte respawn udefra (f.eks. fra PauseMenuController)
    public void StartRespawn()
    {
        StartCoroutine(SmoothRespawn());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnTrigger")) // If player falls or enters respawn zone
        {
            StartCoroutine(SmoothRespawn());
            Debug.Log("Player entered the respawn trigger!");
        }
    }

    private IEnumerator SmoothRespawn()
    {
        Debug.Log($"SmoothRespawn started. Target position: {respawnPosition}");

        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false; // Disable movement control
            Debug.Log("CharacterController disabled for respawn");
        }

        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, respawnPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = respawnPosition; // Ensure exact final position
        Debug.Log($"SmoothRespawn completed. Final position: {transform.position}");

        if (controller != null)
        {
            controller.enabled = true; // Re-enable controller
            Debug.Log("CharacterController re-enabled after respawn");
        }
    }
}