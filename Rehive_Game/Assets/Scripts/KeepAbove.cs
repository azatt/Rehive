using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAbove: MonoBehaviour
{
    public float offsetFromTree = 0.04f;
    public float maxRayDist = 0.1f;
    public GameObject intersected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 origin = transform.position;
        Vector3 dir = transform.right;
        RaycastHit hit;

        //Debug.DrawRay(origin, dir * maxRayDist, Color.red);

        if (Physics.Raycast(origin, dir, out hit, maxRayDist) && hit.transform.tag != "player" )
        {
            intersected = hit.collider.gameObject;
            Vector3 normalized = dir.normalized;
            Vector3 toBeMoved = dir.normalized * (hit.distance - offsetFromTree) ;
            transform.position += toBeMoved;

        }

    }

        
}
