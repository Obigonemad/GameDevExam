using System.Collections;
using UnityEngine;

public class DarkDungeonEffect : MonoBehaviour
{
    public Camera playerCamera;  // Referencen til spillerens kamera
    public float normalFOV = 60f;  // Normal synsfelt
    public float darkDungeonFOV = 20f;  // Begrænset synsfelt i dungeon
    public float smoothSpeed = 5f;  // Hvor hurtigt FOV ændrer sig

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dungeon"))  // Tjekker om spilleren er i dungeon-området
        {
            StartCoroutine(ChangeFOV(darkDungeonFOV));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dungeon"))
        {
            StartCoroutine(ChangeFOV(normalFOV));
        }
    }

    private IEnumerator ChangeFOV(float targetFOV)
    {
        float startFOV = playerCamera.fieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed);
            timeElapsed += Time.deltaTime * smoothSpeed;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;  // Sæt den ønskede FOV
    }
}