using System.Collections;
using UnityEngine;

public class StartChaseTrigger : MonoBehaviour
{
    public EnemyFollow enemyFollowScript;  // Reference til fjendens script

    private bool hasTriggered = false;  // Variabel til at sikre, at triggeren kun aktiveres én gang

    private void OnTriggerEnter(Collider other)
    {
        // Når spilleren træder ind i triggeren, begynd at jage
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;  // Sørg for, at denne del kun aktiveres én gang
            StartCoroutine(StartChaseWithDelay());  // Start jagten med en lille forsinkelse
            Debug.Log("Fjenden begynder at jage spilleren!");
        }
    }

    // Start jagten efter en kort forsinkelse
    private IEnumerator StartChaseWithDelay()
    {
        yield return new WaitForSeconds(1f);  // Forsinkelse på 1 sekund før jagten starter
        enemyFollowScript.StartChasing();  // Start jagten
    }
}