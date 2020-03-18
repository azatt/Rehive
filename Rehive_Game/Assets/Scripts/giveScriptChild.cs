using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveScriptChild : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            child.gameObject.AddComponent<Bird>();
        }
    }
}