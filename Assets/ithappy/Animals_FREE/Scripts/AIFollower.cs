using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIFollower : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign player in Inspector
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            enabled = false; // Disable script to avoid errors
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
    }

    private void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
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
}