using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme_Kontrol : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody Drone1;

    float forward_backward_angle=0, right_left_angle=0 ,angle = 25;
    [SerializeField]
    public Vector3 cross = new Vector3(1,0,1);
    float speed = 91.8f; 
    void Awake(){
        Drone1 = GetComponent<Rigidbody>();
    }
    
    void Start(){

    }

    void update(){

    }

    void FixedUpdate(){
        MovementUpDown();
        Drone1.AddRelativeForce(Vector3.up * speed);
    }

    void MovementUpDown(){

        if(Input.GetKey(KeyCode.UpArrow)){
            speed = 450;}

        else if(Input.GetKey(KeyCode.DownArrow)){
            speed = -200;
        }
        else{
            speed = 98.1f;
        }}





}
