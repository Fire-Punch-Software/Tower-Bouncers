using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //[Header("Movement Settings")]
    //[SerializeField] float walkSpeed = 1f;
    //[SerializeField] float runSpeed = 3f;
    //[SerializeField] float jumpSpeed = 3f;

    //[Header("Combat Settings")]
    //[SerializeField] Transform punchHit;
    //[SerializeField] float punchHitDuration = 0.25f;

    //protected Rigidbody2D rb2D;
    //Animator animator;
    //SpriteRenderer spriteRenderer;

    //protected virtual void Awake()
    //{
    //    rb2D = GetComponent<Rigidbody2D>();
    //    animator = GetComponent<Animator>();
    //    spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    //protected Vector2 desiredMove = Vector2.zero;
    //protected bool mustRun = false;
    //protected bool mustJump = false;
    //protected bool mustPunch = false;
    //protected float actualWalkSpeed = 2f;
    //protected virtual void Update()
    //{
    //    rb2D.linearVelocityX = desiredMove.x * actualWalkSpeed;

    //    if (desiredMove.x != 0f)
    //    {
    //        if (mustRun)
    //        {
    //            animator.SetBool("IsRunning", true);
    //            animator.SetBool("IsWalking", false);
    //            actualWalkSpeed = runSpeed;
    //        } else
    //        {
    //            animator.SetBool("IsWalking", true);
    //            animator.SetBool("IsRunning", false);
    //            actualWalkSpeed = walkSpeed;
    //        }
    //    } else
    //    {
    //        animator.SetBool("IsWalking", false);
    //        animator.SetBool("IsRunning", false);
    //        actualWalkSpeed = walkSpeed;
    //    }

    //    if (desiredMove.x < 0)
    //    {
    //        // spriteRenderer.flipX = true;
    //        transform.localScale = new Vector3(-1f, 1f, 1f);
    //    } else if (desiredMove.x > 0)
    //    {
    //        // spriteRenderer.flipX = false;
    //        transform.localScale = Vector3.one;
    //    }

    //    if (mustJump)
    //    {
    //        mustJump = false;
    //        rb2D.linearVelocityY = jumpSpeed;
    //        animator.SetTrigger("PerformJump");
    //    }
        
    //    if (mustPunch)
    //    {
    //        mustPunch = false;
    //        animator.SetTrigger("PerformPunch");
    //    }
    //}

    //protected void PerformPunch()
    //{
    //    mustPunch = true;
    //    punchHit.gameObject.SetActive(true);
    //    Invoke(nameof(DeactivatePunchHit), punchHitDuration);
    //}

    //private void DeactivatePunchHit()
    //{
    //    punchHit.gameObject.SetActive(false);
    //}

    public virtual void NotifyHit(HitBox2D hitBox2D)
    {
        Debug.Log("hit");
        HealthController health = gameObject.GetComponent<HealthController>();
        health.GetDamage(hitBox2D.damage);
    }
}
