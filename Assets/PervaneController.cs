using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PervaneController : MonoBehaviour
{
    public float rotationSpeed = 100f; // Dönüş hızı

    void Update()
    {
        // Yatay eksende döndürme
        float rotateAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateAmount);
    }
}
