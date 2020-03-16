using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private StatsController checker;
    public GameObject message;

    public int checkstate;

    public bool IsVlinder;
    // Start is called before the first frame update
    void Start()
    {
        checker = FindObjectOfType<StatsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (checker.playerStats.size >= checkstate)
            {
                IsVlinder = true;
                Debug.Log("Become Butterfly");
            }
            else
            {
                message.gameObject.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            message.gameObject.SetActive(false);
        }
    }
}