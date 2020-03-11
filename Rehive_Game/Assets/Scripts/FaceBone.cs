using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBone : MonoBehaviour
{
    public Transform CurrentBone, FollowingBone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FaceObject(CurrentBone, FollowingBone.position);
    }

    private static void FaceObject(Transform scrObject, Vector3 targetPos)
    {
        scrObject.rotation = Quaternion.LookRotation(scrObject.position - targetPos);
    }
}
