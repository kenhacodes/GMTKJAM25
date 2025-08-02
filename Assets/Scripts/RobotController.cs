using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class RobotController : MonoBehaviour
{
    public float health = 100.0f;
    public float maxHealth = 100.0f;
    public float rotationSpeed = 5.0f;
    public bool player = false;
    public RobotHealth healthBar;

    public bool isInvincible = false;
    public float invincibleTime = 0.5f;

    public float floorHeight = 0.0f;
    public float gravityScale = 5.0f;

    public float blockDuration = 0.5f;

    public float normalYKnockback = 1.0f;
    private float currentYKnockback;

    public float normalKnockback = 5.0f;
    private float currentKnockback;

    private bool isInImpactFrame = false;
    private Vector3 accumulatedVelocity = Vector3.zero;
    private Vector3 finalKnockbackAfterTheImpactFrameBecauseICan = Vector3.zero * 3.14f;

    public Transform tr;
    public Rigidbody rb;
    public float moveImpulse = 6.0f;


    private GameObject enemyRobot;

    public BoxCollider hurtboxTop;
    public BoxCollider hurtboxLow;

    public BoxCollider hitBoxTop;
    public BoxCollider hitBoxLow;
    public BoxCollider hitBoxUppercut;
    public BoxCollider hitBoxLeft;
    public BoxCollider hitBoxRight;

    public ParticleSystem vfx_sparks;

    public Transform tr_key;
    private float tr_key_currentRotationX = 0f;
    private Tween keyrotationTween;

    [SerializeField] private AudioSource soundFXObject;
    public float SFXSoundLevelVolume = 1.0f;
    public AudioClip[] soundsDamageColision;
    public AudioClip[] soundsWhoosh;
    public AudioClip[] soundsNormalColision;

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
        InitRobot();
    }

    public void InitRobot()
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
        hitBoxUppercut.enabled = false;
        hitBoxLeft.enabled = false;
        hitBoxRight.enabled = false;
    }

    private void FixedUpdate()
    {
        if (isInImpactFrame)
        {
            accumulatedVelocity += rb.velocity;
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.AddForce(Physics.gravity * (rb.mass * (gravityScale - 1)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;

        if (player)
        {
            //Testing 
            if (Input.GetMouseButtonDown(0))
            {
                RobotKeyRotation();
                //LookAtEnemy();
                //JumpFront();
                AttackNormal();
                LookAtEnemy();
            }

            if (Input.GetMouseButtonDown(1))
            {
                RobotKeyRotation();
                MoveRobot(Directions.Backwards);
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


        if (tr.position.y < floorHeight + 1.5f) dir.y = 0.33f;

        rb.AddForce(dir * moveImpulse, ForceMode.Impulse);
    }

    public void AttackNormal()
    {
        PlayeWooshSound();
        MoveRobot(Directions.Forward, 0.5f);
        StartCoroutine(ActivateHitbox(hitBoxTop, 1.0f));
    }

    public void AttackLow()
    {
        PlayeWooshSound();
        MoveRobot(Directions.Forward, 0.5f);
        StartCoroutine(ChangeYKnockback(2.0f, 1.0f));
        StartCoroutine(ChangeKnockback(normalKnockback + 2.0f, 1.0f));
        StartCoroutine(ActivateHitbox(hitBoxLow, 1.0f));
    }

    public void BlockNormal()
    {
        rb.velocity = Vector3.zero;
        StartCoroutine(DeactivateHurtbox(hurtboxTop, blockDuration));
    }

    public void BlocKLow()
    {
        rb.velocity = Vector3.zero;
        StartCoroutine(DeactivateHurtbox(hurtboxLow, blockDuration));
    }

    public void JumpFront()
    {
        Vector3 dir;
        if (tr.position.y < floorHeight + 2.0f)
        {
            dir = tr.forward.normalized * moveImpulse;
            dir.y = 15.5f;
        }
        else
        {
            dir = tr.forward.normalized * moveImpulse * 0.5f;
        }

        rb.AddForce(dir, ForceMode.Impulse);
    }

    public void AttackLeft()
    {
        PlayeWooshSound();
        StartCoroutine(ActivateHitbox(hitBoxLeft, 0.5f));
    }

    public void AttackRight()
    {
        PlayeWooshSound();
        StartCoroutine(ActivateHitbox(hitBoxRight, 0.5f));
    }

    public void AttackUppercut()
    {
        PlayeWooshSound();
        StartCoroutine(ChangeYKnockback(10.0f, 0.5f));
        StartCoroutine(ActivateHitbox(hitBoxUppercut, 0.5f));
    }

    // Other functions
    public void RobotKeyRotation()
    {
        if (keyrotationTween != null && keyrotationTween.IsActive())
        {
            keyrotationTween.Kill();
        }

        float startRotationX = tr_key.localEulerAngles.x;
        float endRotationX = tr_key_currentRotationX + 12.0f;
        tr_key_currentRotationX = endRotationX;

        keyrotationTween = DOVirtual.Float(startRotationX, endRotationX, 0.3f, value =>
            {
                Vector3 euler = tr_key.localEulerAngles;
                euler.x = value;
                euler.y = 90.0f;
                euler.z = 270.0f;
                tr_key.localEulerAngles = euler;
            })
            .SetEase(Ease.OutBack);
    }

    private void PlayeWooshSound()
    {
        if (soundsWhoosh.Length > 0)
        {
            PlaysSound(soundsWhoosh[Random.Range(0, soundsWhoosh.Length - 1)], 0.2f, 0.6f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hurtbox"))
        {
            Debug.Log("Hitbox collided with hurtbox!");

            // Optional: access enemy script
            var enemy = other.GetComponentInParent<RobotController>();
            if (enemy != null)
            {
                enemy.TakeDamage(10.0f, currentKnockback, currentYKnockback);
                StartCoroutine(enemy.ImpactFrame());
                StartCoroutine(ImpactFrame());
            }
        }
    }

    public void TakeDamage(float damage, float knockbackForce = 5.0f, float knockbackForceY = 1.0f)
    {
        if (isInvincible) return;

        health -= damage;
        health = Mathf.Max(0.0f, health);

        Vector3 knockback_dir = Vector3.zero;
        knockback_dir = -(enemyRobot.transform.position - tr.position).normalized;
        knockback_dir.y = knockbackForceY;
        StartCoroutine(ActivateInvincibility());
        finalKnockbackAfterTheImpactFrameBecauseICan = knockback_dir * knockbackForce;
        vfx_sparks.Play();
        if (soundsDamageColision.Length > 0)
        {
            PlaysSound(soundsDamageColision[Random.Range(0, soundsDamageColision.Length - 1)], 0.15f, 1.0f);
        }
    }

    public void LookAtEnemy()
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

    public void PlaysSound(AudioClip audioClip, float pitchRange, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, tr.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.pitch = Random.Range(audioSource.pitch - pitchRange, audioSource.pitch + pitchRange);
        audioSource.volume = volume * SFXSoundLevelVolume;
        Debug.Log("Final volume: " + audioSource.volume + " SFXLevelVolume: " + SFXSoundLevelVolume);
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }


    // Coroutines

    IEnumerator ImpactFrame()
    {
        isInImpactFrame = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        yield return new WaitForSeconds(0.2f);

        //rb.velocity = velocity_angular;
        rb.useGravity = true;
        rb.AddForce(finalKnockbackAfterTheImpactFrameBecauseICan, ForceMode.Impulse);
        isInImpactFrame = false;
    }

    IEnumerator ActivateHitbox(BoxCollider hitbox, float activeDuration = 1.0f)
    {
        hitbox.enabled = true;
        yield return new WaitForSeconds(activeDuration);
        hitbox.enabled = false;
    }

    IEnumerator DeactivateHurtbox(BoxCollider hitbox, float activeDuration = 1.0f)
    {
        hitbox.enabled = false;
        yield return new WaitForSeconds(activeDuration);
        hitbox.enabled = true;
    }

    IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
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