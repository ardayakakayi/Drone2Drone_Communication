using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadLocations : MonoBehaviour
{
    public static List<Vector3> LoadPositions(string filePath)
    {
        List<Vector3> positions = new List<Vector3>();

        try
        {
            // CSV dosyasını satır satır oku
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                // Her satırı virgül ile ayırarak sütunlara böl
                string[] values = line.Split(',');

                if (values.Length >= 3)
                {
                    // x, y ve z değerlerini alarak Vector3 oluştur
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);
                    float z = float.Parse(values[2]);

                    Vector3 position = new Vector3(x, y, z);

                    // Listeye ekle
                    positions.Add(position);
                }
                else
                {
                    Debug.LogError("CSV dosyasındaki bir satır eksik değere sahip.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("CSV dosyası okunurken bir hata oluştu: " + e.Message);
        }

        return positions;
    }
}