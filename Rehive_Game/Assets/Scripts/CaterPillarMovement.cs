using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterPillarMovement : MonoBehaviour
{
    [SerializeField] public float initialSpeed;
    [SerializeField] public float climbSpeed = 1;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * climbSpeed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * climbSpeed * Time.deltaTime);           ;
    }
}
