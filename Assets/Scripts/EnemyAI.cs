using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign player in Inspector
    [SerializeField] private GameObject objectToDrop; // Object to drop
    [SerializeField] private AudioClip dropSound; // Sound effect when object drops
    [SerializeField] private float dropHeight = 1.5f; // Distance below the enemy to drop the object
    [SerializeField] private float reachDistance = 1.5f; // Distance to trigger event
    [SerializeField] private float resumeChaseDelay = 1.5f; // Delay before resuming chase
    [SerializeField] private float despawnTime = 5f; // Time before the skeleton disappears
    private NavMeshAgent navMeshAgent;
    private bool hasReachedTarget = false; // Prevents multiple triggers

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            enabled = false;
            return;
        }

        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError(gameObject.name + " is not placed on a valid NavMesh!");
            enabled = false;
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player reference is missing! Assign it in the Inspector.");
            enabled = false;
        }

        if (objectToDrop == null)
        {
            Debug.LogError("Object to drop is missing! Assign it in the Inspector.");
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
            Debug.LogWarning("NavMeshAgent is not valid or AI is not on a NavMesh!");
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
            Debug.Log($"Target reached! Distance: {distance}. Dropping object...");
            OnTargetReached();
        }
    }

    private void OnTargetReached()
    {
        // Drop the object below the enemy
        DropObject();

        // Stop movement briefly before resuming chase
        navMeshAgent.isStopped = true;

        // Start a coroutine to resume chasing the player after a delay
        StartCoroutine(ResumeChaseAfterDelay());
    }

    private void DropObject()
    {
        if (objectToDrop != null)
        {
            // Drop the object slightly below the enemy position
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y - dropHeight, transform.position.z);
            Debug.Log($"Dropping object at position: {dropPosition}");

            // Instantiate the object at the calculated position
            GameObject droppedObject = Instantiate(objectToDrop, dropPosition, Quaternion.identity);

            if (droppedObject != null)
            {
                Debug.Log("Object instantiated successfully.");
                
                // Play sound effect at drop location
                if (dropSound != null)
                {
                    AudioSource.PlayClipAtPoint(dropSound, dropPosition);
                    Debug.Log("Drop sound played.");
                }
                else
                {
                    Debug.LogWarning("No drop sound assigned!");
                }
                
                // Ensure object has a Rigidbody for physics (gravity)
                Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Debug.Log("Object has Rigidbody. Enabling gravity.");
                    rb.isKinematic = false; // Make sure it's not kinematic so gravity can affect it
                    rb.useGravity = true;   // Ensure that gravity is enabled for the object
                }
                else
                {
                    Debug.LogWarning("Object does not have a Rigidbody component! Gravity will not be applied.");
                }

                // Destroy the dropped object after `despawnTime` seconds
                Destroy(droppedObject, despawnTime);
                Debug.Log($"Object will despawn in {despawnTime} seconds.");
            }
            else
            {
                Debug.LogWarning("Failed to instantiate the object!");
            }
        }
        else
        {
            Debug.LogWarning("No object assigned to drop!");
        }
    }

    private IEnumerator ResumeChaseAfterDelay()
    {
        yield return new WaitForSeconds(resumeChaseDelay);

        // Resume chasing the player
        Debug.Log("Resuming chase...");
        navMeshAgent.isStopped = false;
        hasReachedTarget = false; // Reset for next trigger
    }
}
