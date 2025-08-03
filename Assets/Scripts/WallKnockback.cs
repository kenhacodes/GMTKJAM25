using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKnockback : MonoBehaviour
{
    public float force = 5.0f;
    private Vector3 dir = Vector3.zero;
    
    
    private void Start()
    {
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hurtbox"))
        {
            var robot = other.GetComponentInParent<Rigidbody>();
            if (robot != null)
            {
                dir = transform.forward.normalized;
                dir *= force;
                if (robot.transform.position.y < 2.0f) dir.y = 1.5f; // Todo Change to final floor position + 2.0f
                //Debug.Log("wall "+ this.gameObject.name + ", robot: " + robot.gameObject.name + " -> dir : " + dir);
                robot.velocity = Vector3.zero;
                robot.AddForce(dir, ForceMode.Impulse);
                RobotController controller = other.GetComponentInParent<RobotController>();
                if (controller != null)
                {
                    controller.PlayBoingSound();
                }
                
                dir = Vector3.zero;
            }
        }
    }
}