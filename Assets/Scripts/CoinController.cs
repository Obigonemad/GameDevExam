using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Vector3 rotationAxis = Vector3.forward; // Ændret til z-aksen (forward)

    [Header("Collection Settings")]
    [SerializeField] private int coinValue = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip collectionSound;
    [SerializeField] private GameObject collectionEffect;

    private bool isCollected = false;

    void Update()
    {
        // Få mønten til at spinne rundt
        if (!isCollected)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tjek om det er spilleren der rører ved mønten
        if (!isCollected && other.CompareTag(playerTag))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        isCollected = true;

        // Tilføj point til spilleren gennem GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(coinValue);
        }
        else
        {
            Debug.LogWarning("GameManager ikke fundet! Kontrollér at GameManager er til stede i scenen.");
        }

        // Afspil lyd hvis der er en
        if (collectionSound != null)
        {
            AudioSource.PlayClipAtPoint(collectionSound, transform.position, 1.0f);
        }

        // Vis effekt hvis der er en
        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }

        // Deaktiver visuelle komponenter
        GetComponent<Renderer>().enabled = false;

        // Hvis der er et Collider-komponent, deaktiver det
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = false;
        }

        // Ødelæg objektet efter et kort øjeblik (så lyden kan afspilles færdig)
        Destroy(gameObject, collectionSound != null ? collectionSound.length : 0.1f);
    }
}