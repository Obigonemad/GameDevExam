using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can activate the checkpoint
        {
            PlayerRespawner respawner = other.GetComponent<PlayerRespawner>();
            if (respawner != null)
            {
                respawner.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint updated!");
            }
        }
    }
}