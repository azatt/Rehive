using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollowMe : MonoBehaviour
{
    float rotSpeed;
    Quaternion targetRotation;

    void Start()
    {
        rotSpeed = 100;

    }

    void Update()
    {
       // RotateMove();
    }

    private void RotateMove()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 moveDirection = (Vector3.forward * movementInput.y + Vector3.right * movementInput.x).normalized;
        if (moveDirection.z <= 0)
        {
            targetRotation = Quaternion.Euler(new Vector3(transform.rotation.x + moveDirection.x * 90, transform.rotation.y, transform.rotation.z));
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(180 - moveDirection.x * 90, transform.rotation.y, transform.rotation.z));
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed);
    }
}
