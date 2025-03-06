using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public GameObject playerCamera;  
    public GameObject mainCamera;    

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))  
        {
            // Switch to main camera when the player enters
            playerCamera.SetActive(false);  
            mainCamera.SetActive(true);     

            Debug.Log("Player entered the trigger zone: Main Camera Active");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player left the trigger zone
        if (other.CompareTag("Player"))
        {
            // Switch back to player camera when the player exits
            playerCamera.SetActive(true);  
            mainCamera.SetActive(false);     

            Debug.Log("Player exited the trigger zone: Player Camera Active");
        }
    }
}