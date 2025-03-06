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
        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false; // Disable movement control

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
        controller.enabled = true; // Re-enable controller
    }
}