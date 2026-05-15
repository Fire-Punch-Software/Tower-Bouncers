using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;

public class BaseCharacter : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float linearSpeed = 5f;
    [SerializeField] float jumpSpeed = 3f;
    [SerializeField] float baseGravity = 2f;
    [SerializeField] float maxFallSpeed = 18f;
    [SerializeField] float fallSpeedMultiplier = 2f;

    [Header("Shooting")]
    [SerializeField] GameObject spawnPointTop = null;
    [SerializeField] GameObject slowProjectilePrefab = null;

    protected Rigidbody2D rb2D;
    Animator animator;
    SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
            rb2D.linearVelocityY = jumpSpeed;
            animator.SetTrigger("PerformJump");
        }
        
        Gravity();
    }

    protected void Move(Vector2 direction)
    {
        bool isShooting = animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot");

        if (!isShooting)
        {
            rb2D.position += lastMoveDirection * linearSpeed * Time.deltaTime;
            lastMoveDirection = direction;
        }
    }

    internal void NotifyPunch()
    {
        Destroy(gameObject);
    }

    private void Gravity() {
        if (rb2D.linearVelocity.y < 0)
        {
            rb2D.gravityScale = baseGravity * fallSpeedMultiplier;
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, Mathf.Max(rb2D.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb2D.gravityScale = baseGravity;
        }
    }

}