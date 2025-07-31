using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public RobotHealth healthBar;

    public Transform tr;
    public Rigidbody rb;
    public float moveImpulse = 10.0f;

    public enum Directions
    {
        Forward,
        Backwards,
        Right,
        Left
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        if (rb == null) Debug.Log("RB not set Robot");
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;

        //Testing damage
        if (Input.GetMouseButtonDown(0))
        {
            //TakeDamege(10.0f);
            MoveRobot(Directions.Backwards);
        }
    }

    public void MoveRobot(Directions dr)
    {
        switch (dr)
        {
            case Directions.Forward:
            {
                rb.AddForce(tr.forward.normalized * moveImpulse, ForceMode.Impulse);
                break;
            }
            case Directions.Backwards:
            {
                rb.AddForce(tr.forward.normalized * -moveImpulse, ForceMode.Impulse);
                break;
            }
            case Directions.Left:
            {
                rb.AddForce(tr.forward.normalized * moveImpulse, ForceMode.Impulse);
                break;
            }
            case Directions.Right:
            {
                rb.AddForce(tr.forward.normalized * moveImpulse, ForceMode.Impulse);
                break;
            }
        }
    }

    public void TakeDamege(float damage)
    {
        health -= damage;
        health = Mathf.Max(0.0f, health);
    }
}