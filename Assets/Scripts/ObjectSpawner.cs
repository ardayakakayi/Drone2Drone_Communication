using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform prefabToSpawn;
    public int objectCount = 60;
    public float spawnRadius = 100;

    void Start()
    {
        for (int loop = 0; loop < objectCount; loop++)
        {
            Vector3 spawnPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            Instantiate(prefabToSpawn, spawnPoint, Random.rotation);
        }
    }
}
