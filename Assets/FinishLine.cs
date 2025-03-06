using UnityEngine;
using TMPro;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;  // UI tekstfelt til "Finished!"
    [SerializeField] private string playerTag = "Player";  // Spilleren skal have dette tag

    private void OnTriggerEnter(Collider other)
    {
        // Kun reager, hvis spilleren rører triggeren
        if (other.CompareTag(playerTag))
        {
            textMesh.text = "Finished!";
            Debug.Log($"{other.gameObject.name} finished the course!");
        }
    }
}