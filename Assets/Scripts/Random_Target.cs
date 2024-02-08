using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Target : MonoBehaviour
{
 public GameObject cubePrefab; 


    public delegate void SpawnCompleteDelegate();
    public static event SpawnCompleteDelegate OnSpawnComplete;
 void Start()
    {
        StartCoroutine(SpawnObjectsCoroutine());

       

    }

IEnumerator SpawnObjectsCoroutine()
    {
       // CSV dosyasının yolu
        string filePath = @"D:\Users\Scott FRANCO\Drones_Project\Assets\ToRead\Locations.csv";

        // ReadLocations sınıfındaki LoadPositions metodunu kullanarak pozisyonları oku
        List<Vector3> positions = ReadLocations.LoadPositions(filePath);
        // Liste üzerinde dolaşarak pozisyonları kullan
        foreach (Vector3 position in positions)
        {
            Vector3 randomSpawnPosition = new Vector3(position.x, position.y, position.z);
            GameObject newOne = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);  
            newOne.tag="TargetClone";

            // Yield bir frame bekleyerek diğer sınıfın Awake ve Start metodlarına izin verir.
        yield return null;

        // Spawn işlemi tamamlandığında event'i tetikle
        if (OnSpawnComplete != null)
        {
            OnSpawnComplete();
        }
    }
    }

}

