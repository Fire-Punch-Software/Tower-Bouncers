using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//using static UnityEditor.PlayerSettings.SplashScreen;

public class BaseCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float linearSpeed = 1.2f;
    [SerializeField] float jumpSpeed = 2.2f;
    [SerializeField] float baseGravity = 0.65f;
    [SerializeField] float maxFallSpeed = 6f;
    [SerializeField] float fallSpeedMultiplier = 0.6f;

    [Header("Shooting")]
    [SerializeField] GameObject spawnPointTop = null;
    [SerializeField] GameObject slowProjectilePrefab = null;

    protected Rigidbody2D rb2d;
    Animator animator;
    SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected Vector2 lastMoveDirection = Vector2.zero;
    protected bool mustJump = false;
    protected bool mustShoot = false;
    protected virtual void Update()
    {
        animator.SetBool("IsWalking", false);
        if (lastMoveDirection.x < 0)
        {
            // spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-1f, 1f, 1f);
            animator.SetBool("IsWalking", true);
        }
        else if (lastMoveDirection.x > 0)
        {
            animator.SetBool("IsWalking", true);
            transform.localScale = Vector3.one;
        }

        if (mustShoot && spawnPointTop)
        {
            mustShoot = false;
            animator.SetTrigger("PerformShoot");
            Instantiate(slowProjectilePrefab, spawnPointTop.transform.position, Quaternion.identity);
        }

        if (mustJump)
        {
            mustJump = false;
            rb2d.linearVelocityY = jumpSpeed;
            animator.SetTrigger("PerformJump");
        }
        
        Gravity();
    }

    protected void Move(Vector2 direction)
    {
        bool isShooting = animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot");

        if (!isShooting)
        {
            rb2d.position += lastMoveDirection * linearSpeed * Time.deltaTime;
            lastMoveDirection = direction;
        }
    }

    internal void NotifyPunch()
    {
        Destroy(gameObject);
    }

    private void Gravity() {
        if (rb2d.linearVelocity.y < 0)
        {
            rb2d.gravityScale = baseGravity * fallSpeedMultiplier;
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, Mathf.Max(rb2d.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb2d.gravityScale = baseGravity;
        }
    }

    public void NotifyHit(HitBox2D hitBox2D)
    {
        Debug.Log("hit");
        Animator animator = gameObject.GetComponent<Animator>();
        StartCoroutine(BlinkRed());

        HealthController health = gameObject.GetComponent<HealthController>();
        health.GetDamage(hitBox2D.damage);

        if (animator.GetBool("IsDead"))
        {
            Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}