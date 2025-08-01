using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKnockback : MonoBehaviour
{
    public float force = 5.0f;
    private Vector3 dir = Vector3.zero;

    private RobotController[] enemyRobots;


    private void Start()
    {
        SearchForRobots();
    }

    private void SearchForRobots()
    {
        enemyRobots = FindObjectsOfType<RobotController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox"))
        {
            
            var robot = other.GetComponentInParent<Rigidbody>();
            if (robot != null)
            {
                dir = transform.forward.normalized;
                dir.y = 1.5f;
                dir *= force;
                Debug.Log("wall "+ this.gameObject.name + ", robot: " + robot.gameObject.name + " -> dir : " + dir);
                robot.velocity = Vector3.zero;
                robot.AddForce(dir, ForceMode.Impulse);
                dir = Vector3.zero;
            }
            
            
        }
    }
}