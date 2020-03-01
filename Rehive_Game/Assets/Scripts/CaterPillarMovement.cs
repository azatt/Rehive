using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterPillarMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float initialSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * speed * Time.deltaTime);           ;
    }
}
