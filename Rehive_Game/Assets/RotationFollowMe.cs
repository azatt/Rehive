using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowMe : MonoBehaviour
{
    float rotSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 moveDirection = (Vector3.forward * movementInput.y + Vector3.right * movementInput.x).normalized;
        //print(Quaternion.LookRotation(moveDirection).x +" "+ Quaternion.LookRotation(moveDirection).y + " " + Quaternion.LookRotation(moveDirection).z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position), rotSpeed);
    }
}
