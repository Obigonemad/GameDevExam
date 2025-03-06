using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // Rock prefab to pool
    public int initialSize = 10;
    public bool expandable = true; // If true, creates new objects when needed
    public Vector3 spawnAreaMin; // Minimum spawn position
    public Vector3 spawnAreaMax; // Maximum spawn position
    public float spawnInterval = 1f; // Time interval between spawns
    public float fallSpeed = 5f; // Speed at which rocks fall

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        StartCoroutine(SpawnRocksContinuously());
    }

    private IEnumerator SpawnRocksContinuously()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject rock = GetObject();
            if (rock != null)
            {
                StartCoroutine(FallDown(rock));
            }
        }
    }

    private IEnumerator FallDown(GameObject rock)
    {
        while (rock.activeSelf && rock.transform.position.y > spawnAreaMin.y)
        {
            rock.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        ReturnObject(rock);
    }

    public GameObject GetObject()
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else if (expandable)
        {
            obj = Instantiate(prefab);
        }
        else
        {
            return null;
        }

        obj.SetActive(true);
        obj.transform.position = GetRandomSpawnPosition();
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = spawnAreaMax.y; // Start from the top
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }
}
