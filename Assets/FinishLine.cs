using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // For TextMeshPro UI

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject gameFinishedUI;
    [SerializeField] private TextMeshProUGUI finalTimeText; // Reference til tekst der viser endelig tid

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // Stop timeren og f� den endelige tid
                if (GameTimer.Instance != null)
                {
                    GameTimer.Instance.StopTimer();
                    string finalTime = GameTimer.Instance.GetTimeString();

                    // Vis UI med den endelige tid
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
            }
        }
    }
}