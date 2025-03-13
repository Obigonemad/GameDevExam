using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"Checkpoint initialized at {transform.position} in scene {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can activate the checkpoint
        {
            Debug.Log($"Player entered checkpoint at {transform.position}");

            // Opdater PlayerRespawner
            PlayerRespawner respawner = other.GetComponent<PlayerRespawner>();
            if (respawner != null)
            {
                respawner.SetCheckpoint(transform.position);

                // Opdater ogs� PauseMenuController
                if (PauseMenuController.Instance != null)
                {
                    PauseMenuController.Instance.SetLastCheckpoint(transform);
                    Debug.Log("PauseMenuController checkpoint also updated");
                }

                Debug.Log("Checkpoint updated in both systems!");
            }
            else
            {
                Debug.LogError("Player does not have PlayerRespawner component!");
            }
        }
    }
}