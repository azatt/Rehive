using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnim : MonoBehaviour
{
    public List<Transform> BodyParts = new List<Transform>();
    
    public float minDistance;
    public float speed;
    public float rotationspeed;

    bool moving;
    float curSpeed;
    float distance;
    Transform curBodyPart;
    Transform prevBodyPart;

    private float xPos, timer;
    public float animSpeed, frequency, amplitude, contractDelay;

    void Update()
    {
        Move();
        Animation();
        WaveMotion();
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            curSpeed = speed;
            moving = true;

            BodyParts[0].Translate(-BodyParts[0].up * curSpeed * Time.smoothDeltaTime, Space.World);

            if (Input.GetAxis("Horizontal") != 0)
                BodyParts[0].Rotate(Vector3.left * rotationspeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }
        else
        {
            moving = false;
            for (int i = 1; i < BodyParts.Count; i++)
            {
                Animator anim = BodyParts[i].gameObject.GetComponent<Animator>();
                anim.SetInteger("i", 0);
            }
        }
    }

    public void Animation()
    {
        if (moving)
        {
            for (int i = 1; i < BodyParts.Count; i++)
            {
                curBodyPart = BodyParts[i];
                prevBodyPart = BodyParts[i - 1];

                distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

                float T = Time.deltaTime * distance / minDistance * curSpeed;

                if (T > 0.5f)
                    T = 0.5f;
                curBodyPart.position = Vector3.Slerp(curBodyPart.position, prevBodyPart.position, T);
                curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, prevBodyPart.rotation, T);

                Animator anim = BodyParts[i].gameObject.GetComponent<Animator>();
                anim.SetInteger("i", i);
            }
        }
    }

    public void WaveMotion()
    {
        if (moving)
        {
            timer += Time.smoothDeltaTime;

            for (int i = 1; i < BodyParts.Count; i++)
            {
                float w = frequency * 2 * Mathf.PI;
                float y = amplitude * Mathf.Cos(w * (timer - (i / animSpeed) * contractDelay));

                foreach (Transform child in BodyParts[i].transform)
                {
                    child.localPosition = new Vector3(child.localPosition.x, y, child.localPosition.z);
                }
            }
        }
    }
}
