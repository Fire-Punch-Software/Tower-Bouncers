using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings.SplashScreen;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] float linearSpeed = 5f;

    Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    protected Vector2 lastMoveDirection = Vector2.zero;
    protected virtual void Update()
    {
        if (lastMoveDirection.x < 0)
        {
            // spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (lastMoveDirection.x > 0)
        {
            // spriteRenderer.flipX = false;
            transform.localScale = Vector3.one;
        }
    }

    protected void Move(Vector2 direction)
    {
        rb2D.position += lastMoveDirection * linearSpeed * Time.deltaTime;
        lastMoveDirection = direction;
    }

    internal void NotifyPunch()
    {
        Destroy(gameObject);
    }
}
