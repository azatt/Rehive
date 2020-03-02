using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public Transform Playertransform;
    public Vector3 PosOffset;
    public int rx;
    public int ry;
    public int rz;
    void Update()
    {
        Quaternion offset = Quaternion.Euler(rx, ry, rz);
        Quaternion zooi;
        zooi = Playertransform.rotation;
        Debug.Log(zooi);
        zooi.x = offset.x;
        Debug.Log(zooi);
        transform.position = Playertransform.position + PosOffset;
        transform.rotation = zooi;
    }
}
