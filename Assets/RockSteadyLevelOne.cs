using UnityEngine;

public class RockSteadyLevelOne : MonoBehaviour
{
    public Animator rockAnimator; // Drag your rock's Animator here in Unity

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure your player has the "Player" tag
        {
            rockAnimator.SetTrigger("StartMove"); // Trigger animation
        }
    }
}
