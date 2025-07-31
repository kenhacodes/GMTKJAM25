using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotHealth : MonoBehaviour
{
    public Image fill;
    public float changeSpeed = 10.0f;

    public float fillAmount { get; set; } = 1.0f;
    
    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, fillAmount, changeSpeed * Time.deltaTime);
    }
}
