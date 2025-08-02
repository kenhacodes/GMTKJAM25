using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotHealth : MonoBehaviour
{
    public Image fill;
    public float changeSpeed = 10.0f;
    public bool isPlayer = false;
    public float fillAmount { get; set; } = -1.0f;

    private Vector3 _originalPos;
    public Vector3 playerHealthBar = new Vector3(600.0f, -90.0f, 0.0f);
    public Vector3 enemyHealthBar = new Vector3(1200.0f, -90.0f, 0.0f);

    private void Start()
    {
        _originalPos = GetComponent<RectTransform>().anchoredPosition;
    }

    public void UpdatePositionHealthBar()
    {
        RectTransform rect = GetComponent<RectTransform>();

        if (!isPlayer)
        {
            rect.anchoredPosition = enemyHealthBar;
            fill.fillOrigin = 1;
        }
        else
        {
            rect.anchoredPosition = playerHealthBar;
            fill.fillOrigin = 0;
        }
    }

    public void SetVisibility(bool active)
    {
        fill.enabled = active;
        GetComponent<Image>().enabled = active;
    }
    

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, fillAmount, changeSpeed * Time.deltaTime);
    }
}