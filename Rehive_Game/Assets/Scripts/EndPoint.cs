using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndPoint : MonoBehaviour
{
    private StatsController checker;
    public GameObject message;
    public PlayableDirector cutscene;

    public bool IsVlinder;
    public int checkstate;
    
    // Start is called before the first frame update
    void Start()
    {
        checker = FindObjectOfType<StatsController>();
    }

    private void Update()
    {
        if (IsVlinder)
        {
            cutscene.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (checker.playerStats.size >= checkstate)
            {
                IsVlinder = true;
            }
            else
            {
                message.gameObject.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            message.gameObject.SetActive(false);
        }
    }
}