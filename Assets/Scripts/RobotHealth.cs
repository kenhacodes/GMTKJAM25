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
    public float fillAmount { get; set; } = 1.0f;

    private Vector3 originalPos;
    public Vector3 enemyHealthBarOffset = new Vector3(600.0f, 0.0f, 0.0f);

    private void Start()
    {
        originalPos = transform.position;
    }

    public void UpdatePositionHealthBar()
    {
        if (!isPlayer) transform.position = originalPos + enemyHealthBarOffset;
    }

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, fillAmount, changeSpeed * Time.deltaTime);
    }
}