using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone2MovementScript : MonoBehaviour
{
    Rigidbody Drone_2;

    void Awake(){
        Drone_2 = GetComponent<Rigidbody>();
    }


    void FixedUpdate(){
        MovementUpDown();
        MovementForward();
        Rotation();
        ClampingSpeedValues();
        Drone_2.AddRelativeForce(Vector3.up * upForce);
        Drone_2.rotation = Quaternion.Euler(
            new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways)
        );
    }

    public float upForce;
    void MovementUpDown(){
        if(Input.GetKey(KeyCode.Space)){
            upForce = 450;
        }
        else if(Input.GetKey(KeyCode.LeftShift)){
            upForce=-200;
        }
        else if(!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)){
            upForce= 98.1f;
        }
    }


private float movementForwardSpeed = 500.0f;
private float tiltAmountForward = 0;
private float tiltVelocityForward;

void MovementForward(){
    if(Input.GetAxis("Vertical")!= 0){
        Drone_2.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
    }
}

private float wantedYRotation;
private float currentYRotation;
private float rotateAmountByKeys = 2.5f;
private float rotationYVelocity;
void Rotation(){
    if(Input.GetKey(KeyCode.E)){
        wantedYRotation -= rotateAmountByKeys;
    }
    if(Input.GetKey(KeyCode.Q)){
        wantedYRotation += rotateAmountByKeys;
    }
    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
}

private Vector3 velocityToSmoothDampToZero;
void ClampingSpeedValues(){
    if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
        Drone_2.velocity = Vector3.ClampMagnitude(Drone_2.velocity, Mathf.Lerp(Drone_2.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }
    if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f){
        Drone_2.velocity = Vector3.ClampMagnitude(Drone_2.velocity, Mathf.Lerp(Drone_2.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    }
    if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
        Drone_2.velocity = Vector3.ClampMagnitude(Drone_2.velocity, Mathf.Lerp(Drone_2.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
    }
    if(Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f){
        Drone_2.velocity = Vector3.SmoothDamp(Drone_2.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);

    }
}
private float sideMovementAmount = 300.0f;
private float tiltAmountSideways;
private float tiltAmountVelocity;
void Swerve(){
    if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f){
        Drone_2.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovementAmount);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountSideways, -20 * Input.GetAxis("Horizontal"), ref tiltAmountVelocity, 0.1f);
    }
    else{
        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
    }
}

}