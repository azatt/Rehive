using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralAnim : MonoBehaviour
{
    public List<Transform> BodyParts = new List<Transform>();
    
    public float minDistance;
    public float speed;
    public float rotationspeed;
    public GameObject player;
    public float minMovingDistance;

    bool moving;
    float curSpeed;
    float distance;
    float animationtimer;
    Transform curBodyPart;
    Transform prevBodyPart;
    Vector3 playerOldPos;
    Vector3 playerOldScale;

    private float xPos, timer;
    public float animSpeed, frequency, amplitude, contractDelay;

    private void Start()
    {
        playerOldPos = player.transform.position;
        playerOldScale = player.transform.localScale;
        minMovingDistance = 0.0001f;
        animationtimer = 5;
    }

    void Update()
    {
        animationtimer -= Time.deltaTime;
        Move();
        Animation();
        WaveMotion();

        if(playerOldScale != player.transform.localScale)
        {
            LinkParent(false);
            transform.localPosition = BodyParts[0].transform.position;
            LinkParent(true);
            transform.localScale = transform.localScale * (player.transform.localScale.magnitude / playerOldScale.magnitude);
        }
        playerOldScale = player.transform.localScale;
    }

    private void LinkParent(bool connect)
    {

        for(int i = 1; i <  BodyParts.Count; i++) 
        {
            BodyParts[i].parent = connect? transform: null;

        }
    }


    public void Move()
    {
        if (Vector3.Distance(playerOldPos,player.transform.position) > minMovingDistance)
        {
            curSpeed = speed;
            moving = true;
            animationtimer = 5;
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
        playerOldPos = player.transform.position;
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
        
        else if (animationtimer <= 0)
        {
            for (int i = 1; i < BodyParts.Count; i++)
            {
                Animator idle = BodyParts[i].gameObject.GetComponent<Animator>();
                idle.SetInteger("i", -i);
            }
        }
    }

    public void WaveMotion()
    {
        if (moving)
        {
            timer += Time.smoothDeltaTime;

            for (int i = 3; i < BodyParts.Count; i++)
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
