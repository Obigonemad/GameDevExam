using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // Spilleren som fjenden skal følge
    public float moveSpeed = 3f;  // Fjendens bevægelseshastighed
    public float delayTime = 1f;  // Forsinkelsestid (sekunder) før fjenden følger spilleren
    public float followDistance = 1.5f; // Hvor tæt fjenden skal være på spilleren før scenen genstartes
    public Vector3 groundNormal = Vector3.up;  // Normalen for fladen/grenen

    private float timeSinceLastUpdate = 0f;
    private bool isChasing = false;  // Er fjenden begyndt at følge spilleren?

    private void Start()
    {
        // Sørg for, at fjenden har en Collider og at Is Trigger er aktiveret
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true; // Sæt collideren som trigger, så fjenden kan passere igennem objekter

        // Fjenden svæver, så deaktiver gravitation
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // Fjenden skal være kinematisk (så den ikke bliver påvirket af fysik)
    }

    private void Update()
    {
        if (isChasing)
        {
            // Beregn forskellen mellem spillerens og fjendens position
            Vector3 directionToPlayer = player.position - transform.position;

            // Brug Vector3.ProjectOnPlane for at følge spillerens position på både x og z (horisontalt)
            Vector3 directionOnGround = Vector3.ProjectOnPlane(directionToPlayer, groundNormal);

            // Beregn højdeforskellen (y-koordinat) for at sikre at fjenden også bevæger sig vertikalt opad
            float verticalMovement = directionToPlayer.y;

            // Juster bevægelsen baseret på både den horisontale og vertikale retning
            Vector3 finalDirection = directionOnGround + new Vector3(0, verticalMovement, 0);

            // Normaliser retningen for at få en enhedsvektor (så hastigheden ikke bliver afhængig af afstanden)
            finalDirection.Normalize();

            // Brug Lerp for at få en glidende bevægelse mod spillerens position
            transform.position = Vector3.Lerp(transform.position, transform.position + finalDirection, moveSpeed * Time.deltaTime);

            // Sørg for at genstarte, når fjenden er tæt på spilleren
            if (Vector3.Distance(transform.position, player.position) < followDistance)
            {
                RestartLevel();
            }
        }
    }

    // Denne funktion aktiverer forfølgelsen, når spilleren træder ind i trigger-zonen (f.eks. første gren)
    public void StartChasing()
    {
        isChasing = true;  // Start jagten
        Debug.Log("Fjenden er nu begyndt at jage!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Når fjenden rører spilleren, genstart niveauet
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fjenden rørte spilleren! Genstarter niveauet.");
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        // Genstart niveauet, når fjenden rører spilleren
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
