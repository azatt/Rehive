using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_body : MonoBehaviour
{
    public float turnspeed = 10f;
    void Update()
    {
        if (Input.GetKey("f"))
        {
            transform.Rotate(Vector3.back, turnspeed * Time.deltaTime);
        }
        if (Input.GetKey("g"))
        {
            transform.Rotate(-Vector3.back, turnspeed * Time.deltaTime);
        }
    }
}
