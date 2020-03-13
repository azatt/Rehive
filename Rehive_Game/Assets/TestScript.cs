using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {

    }

    public void GetSliderValue (float sliderValue)
    {
        //Debug.Log(sliderValue);
        
        if (sliderValue > 0.5f)
        {
            Debug.Log("boven de helft");
        }
        if (sliderValue > 0.7f)
        { 
            Debug.Log("boven 0.7");
        }
    }
  
}
