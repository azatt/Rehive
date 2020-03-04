using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllingDayNight : MonoBehaviour
{
    //main directional light
    public Light sunLight;
    public Light moonLight;

    //models
    public GameObject moonObject;

    // intensity of the light sources
    public float sunIntensity;
    public float moonIntensity;

    // color of the light sources
    public Color sunDayColor;
    public Color sunNightColor;
    public Color moonDayColor;
    public Color moonNightColor;

    //Speed of the cycle (if you set this to 1 the one hour in the cycle will pass in 1 real life second)
    public float daySpeedMultiplier = 10f;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        sunLight.transform.RotateAround(Vector3.zero, Vector3.right, daySpeedMultiplier * Time.deltaTime);
        sunLight.transform.LookAt(Vector3.zero);
        moonLight.transform.RotateAround(Vector3.zero, Vector3.right, daySpeedMultiplier * Time.deltaTime);
        moonLight.transform.LookAt(Vector3.zero);
        moonObject.transform.RotateAround(new Vector3(200,200,200), Vector3.right, daySpeedMultiplier * Time.deltaTime);
        moonObject.transform.LookAt(Vector3.zero);

        sunLight.intensity = sunIntensity;
        moonLight.intensity = moonIntensity;

        if (sunLight.transform.position.y < 0)
        {
            if (sunLight.color != sunNightColor)
            {
                ChangeColor(sunLight, sunNightColor);
            }
            if (moonLight.color != moonNightColor)
            {
                ChangeColor(moonLight, moonNightColor);
            }
        }
        else
        {
            if (sunLight.color != sunDayColor)
            {
                ChangeColor(sunLight, sunDayColor);
            }
            if (moonLight.color != moonDayColor)
            {
                ChangeColor(moonLight, moonDayColor);
            }
        }
    }

    void ChangeColor(Light light, Color color)
    {
        light.color = Color.Lerp(light.color, color, 0.01f);
    }

}
