using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Text speedText;
    public Text sizeText;
    public Text camoText;
    public Text dangerText;
    public Text threatLevel;

    // Start is called before the first frame update
    void Start()
    {
        speedText = transform.Find("Speed").gameObject.GetComponent<Text>();
        sizeText = transform.Find("Size").gameObject.GetComponent<Text>();
        camoText = transform.Find("Camo").gameObject.GetComponent<Text>();
        dangerText = transform.Find("DangerText").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void changeStatText()
    {
    }
}
