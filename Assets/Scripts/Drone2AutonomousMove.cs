using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone2AutonomousMove : MonoBehaviour
{
    Rigidbody Drone_2;
   
    void Awake(){
        Drone_2 = GetComponent<Rigidbody>();
    }


    void FixedUpdate(){
        
        MovementUpDown();
        MovementForward();
        Rotation();
        Drone_2.AddRelativeForce(Vector3.up * upForce);
        Drone_2.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation,Drone_2.rotation.z));
    }

    public float upForce;
    void MovementUpDown(){
       

        // Set our position as a fraction of the distance between the markers.
        
        if(Input.GetKey(KeyCode.W)){    
            upForce=450;
            Debug.Log(Drone_2.transform.position.y);
                }
        else if(Input.GetKey(KeyCode.S)){
            upForce=-350;
        }
        else if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
            upForce=98.1f;
        }
                
    }
        private float MovementForwardSpeed = 500.0f;
        private float tiltAmountForward = 1;
        private float tiltVelocityForward;
        
        void MovementForward(){
            if(Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.X)){
                Drone_2.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * MovementForwardSpeed);
                tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
            }
        }
        void MovementBackward(){
            if(Input.GetAxis("Vertical") != 0 && Input.GetKey(KeyCode.Z)){
                Drone_2.AddRelativeForce(Vector3.back * Input.GetAxis("Vertical") * MovementForwardSpeed);
                tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, -20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
            }
        }
        private float wantedYRotation;
        private float currentYRotation;
        private float rotateAmountByKeys = 2.5f;
        private float rotationYVelocity;

void Rotation(){
    if(Input.GetKey(KeyCode.Q)){
        wantedYRotation -= rotateAmountByKeys;
    }
    if(Input.GetKey(KeyCode.E)){
        wantedYRotation += rotateAmountByKeys;
    }

    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
}



}

