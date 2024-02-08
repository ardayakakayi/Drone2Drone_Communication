using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Box : MonoBehaviour
{
   public GameObject cubePrefab;

   void Update()
   {
        var position = new Vector3();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-100,110), 300, Random.Range(-100,110));
            Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
        }

        
   }
}
