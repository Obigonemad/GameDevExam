using UnityEngine;
using System.Collections;

public class DisableDollyCam : MonoBehaviour
{
    public float disableTime = 10f;

    void Start()
    {
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }
}