﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAbove: MonoBehaviour
{
    public float offsetFromTree = 0.04f;
    public float maxRayDist = 0.1f;
    public GameObject intersected;

    void Update()
    {
        
        Vector3 origin = transform.position;
        Vector3 dir = transform.right;
        RaycastHit hit;

        //Debug.DrawRay(origin, dir * maxRayDist, Color.red);

        if (Physics.Raycast(origin, dir, out hit, maxRayDist) && hit.transform.tag != "player" && hit.transform.tag != "SafeZone")
        {
            intersected = hit.collider.gameObject;
            Vector3 normalized = dir.normalized;
            Vector3 toBeMoved = dir.normalized * (hit.distance - offsetFromTree) ;
            transform.position += toBeMoved;

        }

    }

        
}
