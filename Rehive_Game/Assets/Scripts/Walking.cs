using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    
    public List<Transform> BodyParts = new List<Transform>();

    public float minDistance;
    public float speed;
    public float rotationspeed;

    float curSpeed;
    float distance;
    bool True = true;
    Transform curBodyPart;
    Transform prevBodyPart;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        True = false;
        Move();
        True = true;
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            curSpeed = speed;

            BodyParts[0].Translate(-BodyParts[0].up * curSpeed * Time.smoothDeltaTime, Space.World);

            if (Input.GetAxis("Horizontal") != 0)
                BodyParts[0].Rotate(Vector3.left * rotationspeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }
        if (Vector3.Distance(BodyParts[0].position, BodyParts[1].position) > 0.1)
        {
            BodyParts[1].position = Vector3.MoveTowards(BodyParts[1].position, BodyParts[0].position, Time.deltaTime * curSpeed);
            BodyParts[1].rotation = Quaternion.RotateTowards(BodyParts[1].rotation, BodyParts[0].rotation, Time.deltaTime * rotationspeed);
        }
        else { curSpeed = 0; }

        for (int i = 2; i < BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            prevBodyPart = BodyParts[i - 1];
            
            distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

            float T = Time.deltaTime * distance / minDistance * curSpeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, prevBodyPart.position, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, T);
        }

        print("stopmoving");
    }
}
