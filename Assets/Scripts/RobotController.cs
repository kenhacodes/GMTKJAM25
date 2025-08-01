using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotController : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float rotationSpeed = 5.0f;
    public bool player = false;
    public RobotHealth healthBar;

    public float normalYKnockback = 1.0f;
    private float currentYKnockback;

    public float normalKnockback = 5.0f;
    private float currentKnockback;

    public Transform tr;
    public Rigidbody rb;
    public float moveImpulse = 6.0f;

    private GameObject enemyRobot;

    public BoxCollider hurtboxTop;
    public BoxCollider hurtboxLow;
    public BoxCollider hitBoxTop;
    public BoxCollider hitBoxLow;

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
        if (enemyRobot == null)
        {
            RobotController[] robots = FindObjectsOfType<RobotController>();
            foreach (RobotController robot in robots)
            {
                if (robot != this)
                {
                    enemyRobot = robot.gameObject;
                    break;
                }
            }
        }

        currentYKnockback = normalYKnockback;
        currentKnockback = normalKnockback;

        healthBar.isPlayer = player;
        healthBar.UpdatePositionHealthBar();

        hurtboxTop.enabled = true;
        hurtboxLow.enabled = true;

        hitBoxTop.enabled = false;
        hitBoxLow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtEnemy();
        healthBar.fillAmount = health / maxHealth;

        if (player)
        {
            //Testing 
            if (Input.GetMouseButtonDown(0))
            {
                //LookAtEnemy();
                AttackNormal();
            }

            if (Input.GetMouseButtonDown(1))
            {
                AttackLow();
            }
        }
    }

    // Programmable Actions

    public void MoveRobot(Directions dr, float forceMultiplier = 1.0f)
    {
        Vector3 dir = Vector3.zero;
        switch (dr)
        {
            case Directions.Forward:
            {
                dir = tr.forward.normalized;
                break;
            }
            case Directions.Backwards:
            {
                dir = -tr.forward.normalized;
                break;
            }
            case Directions.Right:
            {
                dir = tr.right.normalized;
                break;
            }
            case Directions.Left:
            {
                dir = -tr.right.normalized;
                break;
            }
        }

        // TODO Change to current floor height + 0.5f
        if (tr.position.y < 1.5f) dir.y = 0.33f;

        rb.AddForce(dir * moveImpulse, ForceMode.Impulse);
    }

    public void AttackNormal()
    {
        MoveRobot(Directions.Forward, 0.5f);
        StartCoroutine(ActivateHitbox(hitBoxTop, 1.0f));
    }

    public void AttackLow()
    {
        MoveRobot(Directions.Forward, 0.5f);
        StartCoroutine(ChangeYKnockback(1.5f, 1.0f));
        StartCoroutine(ActivateHitbox(hitBoxLow, 1.0f));
    }

    // Other functions

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hurtbox"))
        {
            Debug.Log("Hitbox collided with hurtbox!");

            // Optional: access enemy script
            var enemy = other.GetComponentInParent<RobotController>();
            if (enemy != null)
            {
                enemy.TakeDamage(10.0f, 5.0f, currentYKnockback);
            }
        }
    }

    public void TakeDamage(float damage, float knockbackForce = 5.0f, float knockbackForceY = 1.0f)
    {
        health -= damage;
        health = Mathf.Max(0.0f, health);

        Vector3 knockback_dir = Vector3.zero;
        knockback_dir = -(enemyRobot.transform.position - tr.position).normalized;
        knockback_dir.y = knockbackForceY;

        rb.AddForce(knockback_dir * knockbackForce, ForceMode.Impulse);
    }

    void LookAtEnemy()
    {
        if (enemyRobot == null) return;

        Vector3 direction = enemyRobot.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Coroutines

    IEnumerator ActivateHitbox(BoxCollider hitbox, float activeDuration = 1.0f)
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(activeDuration);
        hitbox.enabled = false;
    }

    IEnumerator ChangeYKnockback(float amount, float time)
    {
        currentYKnockback = amount;
        yield return new WaitForSeconds(time);
        currentYKnockback = normalYKnockback;
    }

    IEnumerator ChangeKnockback(float amount, float time)
    {
        currentKnockback = amount;
        yield return new WaitForSeconds(time);
        currentKnockback = normalKnockback;
    }
}