using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OtherDroneControll : MonoBehaviour
{

    Random_Target beginwith;
    public float rotationSpeed = 1f;
    private float targetAngle;
    private float lastAngle;

    private int tmp_method; 
    public Transform drone_only;
    public int temp = 0;

    public float temp_y = 0f;
    float speed_force = 4f;
     public float ascentSpeed = 4f;
    
    public Rigidbody rb; // RigidBody bileşeni eklenmeli
    public Transform droneTransform;
    public Transform en_yakin;
    public Vector3 en_yakinPos = Vector3.zero;
    public Vector3 tmpVec = Vector3.zero;
    public Quaternion en_yakinRot;
    public Vector3 dronePosition;
    Quaternion targetRotation;
    
    private bool isRotationCompleted = false;
    private bool isForwardCompleted = false;
    private bool isUpCompleted = false;
    private bool isDownCompleted = false;
     private bool isFlying = false;
      bool hasSpawnCompleteListener = false;


    private Quaternion targetQuatRotation;
    private Vector3 hedef_rota;

    float angle = 0f;
 
    float updown, horizontalInput, verticalInput;
    
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        droneTransform = GetComponent<Transform>();
        dronePosition = droneTransform.position;

        targetQuatRotation = Quaternion.Euler(0, 0, 0);
       
         // Spawn işlemi tamamlandığında yapılacak işlemler
       GameObject[] hedefler = GameObject.FindGameObjectsWithTag("TargetClone");

        List<float> distances = new List<float>();

        foreach (GameObject hedef in hedefler)
        {
            // Klonun konumu ile bu nesnenin konumu arasındaki mesafeyi kontrol et
            float mesafe = CalculateDistance(hedef.transform.position, dronePosition);
            distances.Add(mesafe);
            // Eğer klon görme menzili içindeyse
            if (distances.Count == 1)
            {
                en_yakin = hedef.transform;
                en_yakinPos = hedef.transform.position;
                en_yakinRot = hedef.transform.rotation;
                tmpVec = new Vector3(en_yakinRot.x,en_yakinRot.y,en_yakinRot.z);
                
            }
            else
            {
                if (mesafe < CalculateDistance(en_yakinPos, transform.position))
                {
                    en_yakinPos = hedef.transform.position;
                    
                    en_yakinRot = hedef.transform.rotation;
                }
            }
        }



        Debug.Log("Eklendi..." + hedefler.Length);
        Debug.Log("GameObjects: -----" + en_yakinPos);
        tmp_method = 2;
        
        // Dönmeyi sağlama
        targetAngle = CalculateTargetAngle(en_yakinPos, rb.rotation) * Mathf.Rad2Deg;
        // Dronu belli bir açıda döndürme        

        //rb.isKinematic = true;
        tmp_method = 2;

    }
    void Update()
    {
      if (tmp_method == 2){ 
       if (!isFlying)
        {
            temp = 1;
            if (transform.position.y < en_yakinPos.y + 20f)
            {
                // Yüksekliği belirli bir hıza çıkarmak için fiziksel kuvvet uygula
                Debug.Log("Uçuş başladı");
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                rb.AddRelativeForce(Vector3.up * 150);
            }
            else
            {
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.None;
                isFlying = true;
                temp = 3;
                 // Uçuş başladığında kinematik ayarı kaldır
            }
        }
       

        if (temp == 3){
        if(!isRotationCompleted)
                {
                    targetQuatRotation = transform.rotation;
                    Debug.Log("[tmp] dronePosition: " + rb.position);
                    RotateAroundYAxis(en_yakinPos);
                    Debug.Log("[tmp] en_yakinPos: " + en_yakinPos);
                    Debug.Log("[tmp] dronePosition: " + rb.position);
                    
                    if (targetRotation == transform.rotation)
                    {
                        isRotationCompleted = true;
                        Debug.Log("Rotation Completed");
                        temp = 4;
                    }
                }
        }
        
        if (temp == 4){
        if(!isForwardCompleted)
                {
                    Debug.Log("[tmp] dronePosition: " + rb.position);
                    // Öne eğilmeyi kontrol et
                    
                    //Öne Eğimi halletmeye


                    MoveForward();
                    Debug.Log("[tmp] en_yakinPos: " + en_yakinPos);
                    Debug.Log("[tmp] dronePosition: " + rb.position);
                    
                    if (rb.position.x - en_yakinPos.x < 0.5f && rb.position.z - en_yakinPos.z < 0.5f)
                    {
                        
                        
                        isForwardCompleted = true;
                        Debug.Log("Moving Completed");
                        temp = 6;
                        isRotationCompleted = false;
                    }
                }
        }
    if (temp == 6){
        if(!isRotationCompleted)
                {
                    
                    Vector3 targetForward = en_yakin.forward;

                    // Hedefin yatay olarak forward vektörünü al
                    Vector3 targetHorizontalForward = targetForward;
                    targetHorizontalForward.y = 0f; // Yükseklik bileşenini sıfırla

                    // Rigidbody'nin hedefin baktığı yöne doğru bakmasını sağla
                    Quaternion targetRotation = Quaternion.LookRotation(targetHorizontalForward);
                    rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    
                    if (Quaternion.Angle(targetRotation, rb.rotation) < 1f)
                    {
                        rb.isKinematic = false;
                        isRotationCompleted = true;
                        temp = 7;
                    }
                }
        }

        if (temp == 7){
            if (!isDownCompleted)
            {
                
                MoveDown();
                   
                if (transform.position.y - en_yakinPos.y <= 0.065f)

                {   
                    rb.isKinematic = true;
                    Debug.Log("isKinematic: true");
                    rb.constraints = RigidbodyConstraints.None;
                    isDownCompleted = true;
                    temp = 7;
                    
                    
                    isRotationCompleted = false;
                    // Uçuş başladığında kinematik ayarı kaldır
                }
            }

        }
        
}}

    private void RotateAroundYAxis(Vector3 hedef_nesne)
    {
     
        if (isRotationCompleted)
        {
            return;  // Dönme işlemi tamamlandıysa, fonksiyonu bitir
        }

        if (en_yakinPos != null)
        {
            // Hedefin yatay konumunu al
            Vector3 targetPosition = hedef_nesne;
            targetPosition.y = rb.position.y; // Sadece yatay ekseni aynı bırak

            // Rigidbody'nin yatay olarak hedefe doğru bakmasını sağla
            targetRotation = Quaternion.LookRotation(targetPosition - rb.position);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }

        
        
    }

    private void MoveForward()
    {
         
         if (isForwardCompleted)
        {
            return;  // Dönme işlemi tamamlandıysa, fonksiyonu bitir
        }
         
         horizontalInput=0f;
        updown= 0f;
        verticalInput=0.6f;
        
        // Girişlere göre drone'u hareket ettirme
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed_force * Time.deltaTime;
        rb.MovePosition(rb.position + droneTransform.TransformDirection(movement));
        
        
    }

private void MoveUp()
    {
         
       /*  if (isUpCompleted)
        {
            return;  // Dönme işlemi tamamlandıysa, fonksiyonu bitir
        }
         
        horizontalInput=0f;
        updown= 5f;
        verticalInput=0f;
        
        // Girişlere göre drone'u hareket ettirme
        Vector3 movement = new Vector3(0, updown, 0) * speed_force * Time.deltaTime;
        rb.MovePosition(rb.position + droneTransform.TransformDirection(movement));
        
        //rb.AddRelativeForce(Vector3.up * updown);*/
        if (isFlying)
        {
            return;  // Dönme işlemi tamamlandıysa, fonksiyonu bitir
        }
         rb.AddForce(Vector3.up * ascentSpeed);
                
        
    }

private void MoveDown()
    {
         
         if (isDownCompleted)
        {
            return;  // Dönme işlemi tamamlandıysa, fonksiyonu bitir
        }

            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            rb.AddRelativeForce(Vector3.up * 80.000001f);
    }


    private float CalculateTargetAngle(Vector3 targetPosition, Quaternion currentRotation)
    {
       
         // İki vektör arasındaki yatay açıyı bulma

        Vector3 currentPosition = currentRotation.eulerAngles;
        angle = Mathf.Atan2(targetPosition.z - currentPosition.z, targetPosition.x - currentPosition.x);

                
        // -pi ile pi arasında bir değere çevirme
        // Bu sayede daha hassas bir sonuç elde edebilirsiniz
        return angle;

    }

    /*###########################################################      -----------*/
    /*###İki Nokta arası mesafe ölçümü w/Euclidian Calculation###      |   Tick  |*/
    /*###########################################################      -----------*/

    public static float CalculateDistance(Vector3 Target, Vector3 Drone)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(Drone.x - Target.x, 2) + Mathf.Pow(Drone.y - Target.y, 2) + Mathf.Pow(Drone.z - Target.z, 2));
        return distance;
    }
}