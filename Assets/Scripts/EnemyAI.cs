using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player; // Tildel spilleren i Inspector
    [SerializeField] private GameObject objectToDrop; // Objekt der skal droppes
    [SerializeField] private AudioClip dropSound; // Lyd effekt når objektet droppes
    [SerializeField] private float dropHeight = 1.5f; // Afstand under fjenden til at droppe objektet
    [SerializeField] private float reachDistance = 1.5f; // Afstand til at udløse hændelse
    [SerializeField] private float resumeChaseDelay = 1.5f; // Forsinkelse før jagten genoptages
    [SerializeField] private float despawnTime = 5f; // Tid før skelettet forsvinder
    private NavMeshAgent navMeshAgent;
    private bool hasReachedTarget = false; // Forhindrer flere triggere

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent-komponenten mangler på " + gameObject.name);
            enabled = false;
            return;
        }

        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError(gameObject.name + " er ikke placeret på en gyldig NavMesh!");
            enabled = false;
            return;
        }

        if (player == null)
        {
            Debug.LogError("Spillerreference mangler! Tildel den i Inspector.");
            enabled = false;
        }

        if (objectToDrop == null)
        {
            Debug.LogError("Objekt til at droppe mangler! Tildel det i Inspector.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
            CheckIfReachedTarget();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent er ikke gyldig, eller AI'en er ikke på en NavMesh!");
            return;
        }

        navMeshAgent.SetDestination(player.position);
    }

    private void CheckIfReachedTarget()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (!hasReachedTarget && distance <= reachDistance)
        {
            hasReachedTarget = true;
            Debug.Log($"Mål nået! Afstand: {distance}. Dropper objekt...");
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        // Drop objektet under fjenden
        DropObject();

        // Stop bevægelsen kortvarigt før jagten genoptages
        navMeshAgent.isStopped = true;

        // Start en coroutine for at genoptage jagten på spilleren efter en forsinkelse
        StartCoroutine(ResumeChaseAfterDelay());
    }

    private void DropObject()
    {
        if (objectToDrop != null)
        {
            // Drop objektet en smule under fjendens position
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y - dropHeight, transform.position.z);
            Debug.Log($"Dropper objekt på position: {dropPosition}");

            // Instantiér objektet på den beregnede position
            GameObject droppedObject = Instantiate(objectToDrop, dropPosition, Quaternion.identity);

            if (droppedObject != null)
            {
                Debug.Log("Objekt instantiéret succesfuldt.");
                
                // Afspil lydeffekt ved drop-positionen
                if (dropSound != null)
                {
                    AudioSource.PlayClipAtPoint(dropSound, dropPosition);
                    Debug.Log("Drop-lyd afspillet.");
                }
                else
                {
                    Debug.LogWarning("Ingen drop-lyd er tildelt!");
                }
                
                // Sikre at objektet har en Rigidbody til fysik (tyngdekraft)
                Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log("Objektet har en Rigidbody. Aktiverer tyngdekraft.");
                    rb.isKinematic = false; // Sørg for, at det ikke er kinematisk, så tyngdekraften påvirker det
                    rb.useGravity = true;   // Sikre at tyngdekraft er aktiveret for objektet
                }
                else
                {
                    Debug.LogWarning("Objektet har ikke en Rigidbody-komponent! Tyngdekraft vil ikke blive anvendt.");
                }

                // Ødelæg det droppede objekt efter `despawnTime` sekunder
                Destroy(droppedObject, despawnTime);
                Debug.Log($"Objektet vil forsvinde om {despawnTime} sekunder.");
            }
            else
            {
                Debug.LogWarning("Kunne ikke instantiere objektet!");
            }
        }
        else
        {
            Debug.LogWarning("Intet objekt tildelt til at droppe!");
        }
    }

    private IEnumerator ResumeChaseAfterDelay()
    {
        yield return new WaitForSeconds(resumeChaseDelay);

        // Genoptag jagten på spilleren
        Debug.Log("Genoptager jagten...");
        navMeshAgent.isStopped = false;
        hasReachedTarget = false; // Nulstil til næste trigger
    }
}
