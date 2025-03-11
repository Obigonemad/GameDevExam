using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // For TextMeshPro UI
using System.Collections; // Required for coroutines

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject gameFinishedUI;
    [SerializeField] private TextMeshProUGUI finalTimeText; // Reference to text displaying final time
    [SerializeField] private TextMeshProUGUI levelCompleteText; // Reference to text displaying "Level Complete"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player has entered the finish line");
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                StartCoroutine(ShowLevelCompleteAndLoadScene(3f, nextSceneIndex));
            }
            else
            {
                // Stop the timer and get the final time
                if (GameTimer.Instance != null)
                {
                    GameTimer.Instance.StopTimer();
                    string finalTime = GameTimer.Instance.GetTimeString();

                    // Show UI with the final time
                    if (gameFinishedUI != null)
                    {
                        gameFinishedUI.SetActive(true);

                        if (finalTimeText != null)
                        {
                            finalTimeText.text = "Final Time: " + finalTime;
                        }
                    }
                }

                Debug.Log("Game Finished! Final time: " + GameTimer.Instance.GetTimeString());
                StartCoroutine(ShowLevelCompleteAndLoadScene(3f, nextSceneIndex));
            }
        }
    }

    private IEnumerator ShowLevelCompleteAndLoadScene(float delay, int sceneIndex)
    {
        if (levelCompleteText != null)
        {
            levelCompleteText.text = "Level Complete!";
            levelCompleteText.gameObject.SetActive(true);
        }
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
