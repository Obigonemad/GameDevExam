using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For TextMeshPro UI

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject gameFinishedUI;  // Reference to the "Game Over" or "Finish" screen UI
    [SerializeField] private TextMeshProUGUI finalTimeText; // Reference to the TextMeshProUGUI component for the final time

    private void Start()
    {
        // Ensure the gameFinishedUI is hidden at the start
        if (gameFinishedUI != null)
        {
            gameFinishedUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // If there's another scene to load, load it
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // Stop the timer and get the final time
                if (GameTimer.Instance != null)
                {
                    GameTimer.Instance.StopTimer();
                    string finalTime = GameTimer.Instance.GetTimeString();

                    // Display the final time on the UI
                    if (gameFinishedUI != null)
                    {
                        gameFinishedUI.SetActive(true);  // Show the UI

                        if (finalTimeText != null)
                        {
                            finalTimeText.text = "Final Time: " + finalTime;  // Set the final time text
                        }
                    }
                }

                Debug.Log("Game Finished! Final time: " + GameTimer.Instance.GetTimeString());
            }
        }
    }
}
