using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTemp : MonoBehaviour
{
    private Animator idleAnimation;
    private float animationtimer, playingtimer;
    private bool test;

    // Start is called before the first frame update
    void Start()
    {
        idleAnimation = GetComponent<Animator>();
        playingtimer = 11;
    }

    // Update is called once per frame
    void Update()
    {
        animationtimer -= Time.deltaTime;
        playingtimer -= Time.deltaTime;
        if (animationtimer <= 0)
        {
            idleAnimation.SetBool("StandingStill", true);
            if (playingtimer <= 0)
            {
                playingtimer = 11;
                animationtimer = 5;
            }
        }
        else
        {
            idleAnimation.SetBool("StandingStill", false);
        }
    }
    
}
