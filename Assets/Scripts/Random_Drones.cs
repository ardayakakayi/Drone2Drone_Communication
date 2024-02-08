using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Drones : MonoBehaviour
{
   public GameObject droneObj;
    public int i;
    public int count;

    public delegate void SpawnDroneCompleteDelegate();
    public static event SpawnDroneCompleteDelegate OnSpawnComplete;

    void Start()
    {
        StartCoroutine(SpawnDroneCoroutine());

    }

 IEnumerator SpawnDroneCoroutine()
 {
        i = Random.Range(1, 5);
        count = 0;
        Debug.Log("i random değeri: " + i);
        for (int j = 0; j < i; j++)
        {
            var position = new Vector3();
        
            Vector3 randomDronePosition = new Vector3(Random.Range(-100,100), 1, Random.Range(-100,100));
            GameObject newOne = Instantiate(droneObj, randomDronePosition, Quaternion.identity);  
            newOne.tag="DroneClone";
            count++;
        
        }
        
        if (count == i)
        {
            Debug.Log( i + " kadar drone oluşturuldu... ");
        }
        
        yield return null;

        // Spawn işlemi tamamlandığında event'i tetikle
        if (OnSpawnComplete != null)
        {
            OnSpawnComplete();
        }

        
   }
}