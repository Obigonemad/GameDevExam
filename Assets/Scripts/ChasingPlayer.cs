using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // Spilleren som fjenden skal følge
    public float moveSpeed = 3f;  // Fjendens bevægelseshastighed
    public float followDistance = 1.5f; // Hvor tæt fjenden skal være på spilleren før scenen genstartes
    private Vector3 groundNormal = Vector3.up;  // Normalen for fladen/grenen

    // Fjendens ønskede startposition (f.eks. en spec. Vector3)
    private Vector3 enemySpawnPoint = new Vector3(12.9f, 5.34f, 37.8f); 
    private Collider playerCollider;
    private Collider enemyCollider;

    private bool isChasing = false;  // Er fjenden begyndt at følge spilleren?

    private void Start()
    {
        playerCollider = player.GetComponent<Collider>();  // Spilleren's Collider
        enemyCollider = GetComponent<Collider>();  // Fjendens Collider

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
                EnemyCheckpointTrigger();
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
        // Når fjenden rører spilleren, gå til sidste checkpoint
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fjenden rørte spilleren! Tilbage til checkpoint");
            EnemyCheckpointTrigger();
        }

        // Hvis fjenden rører StartPoint (startstub), så genstart fjenden
        if (other.CompareTag("StartPoint"))
        {
            Debug.Log("Fjenden rørte StartPoint! Genstart fjenden.");
            EnemyReset();
        }
    }

    private void EnemyCheckpointTrigger()
    {
        // Genstart niveauet, når fjenden rører spilleren
        PlayerRespawner playerRespawner = player.GetComponent<PlayerRespawner>();
        playerRespawner.StartRespawn();
        
        // Hvis fjenden er tæt på startpositionen, reset fjenden
        if (transform.position == enemySpawnPoint)
        {
            Debug.Log("Fjenden rørte startpositionen - fjenden genstartes!");
            EnemyReset();
        }
    }

 private void EnemyReset()
{
    // Midlertidigt deaktiver Collideren for at undgå kollisioner, når vi flytter fjenden
    Collider coll = GetComponent<Collider>();
    coll.enabled = false;  // Deaktiverer collideren midlertidigt for at undgå kollision

    // Log fjendens nuværende position, før vi sætter den til startpositionen
    Debug.Log("Fjenden position før reset: " + transform.position);

    // Find træstubben og få dens Collider
    Transform treeStump = GameObject.FindWithTag("StartPoint").transform;
    Collider treeStumpCollider = treeStump.GetComponent<Collider>(); // Få Collideren
    float treeStumpHeight = treeStump.position.y + treeStumpCollider.bounds.extents.y; // Juster for colliderens højde

    // Log træstubben's position og dens collider højde
    Debug.Log("Træstubben position: " + treeStump.position);
    Debug.Log("Træstubben højder: " + treeStumpCollider.bounds.extents.y);

    // Juster fjendens position lidt over træstubben's højde
    Vector3 adjustedPosition = new Vector3(treeStump.position.x, treeStumpHeight + 10f, treeStump.position.z);  // Justeret y med +10f for at placere den lidt højere ovenpå

    // Fjenden resettes til den ønskede startposition
    transform.position = adjustedPosition;  // Brug træstubben's x og z, og den justerede y

    // Log fjendens position, efter den er blevet flyttet
    Debug.Log("Fjenden er flyttet til: " + transform.position);

    // Genaktiver Collideren, så fjenden kan begynde at kollidere med andre objekter igen
    coll.enabled = true;  // Genaktiverer collideren

    // Log positionen efter Collideren er genaktiveret
    Debug.Log("Collider genaktiveret. Fjendens position: " + transform.position);

    // Deaktiver StartChaseTrigger midlertidigt, så jagten ikke starter automatisk
    StartChaseTrigger startChaseTrigger = FindObjectOfType<StartChaseTrigger>();
    if (startChaseTrigger != null)
    {
        startChaseTrigger.ResetTrigger();  // Nulstil triggeren for at sikre, at jagten ikke starter med det samme
    }

    // Stop jagten ved at sætte isChasing til false
    isChasing = false;  // Sørg for at stoppe jagten, når fjenden resettes
    Debug.Log("Fjenden jagt stoppet.");
}



}
