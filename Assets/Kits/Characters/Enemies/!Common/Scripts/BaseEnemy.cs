using System;
using UnityEngine;

public class BaseEnemy : MovementController
{
    [Header("Enemy Settings")]
    [SerializeField] int score = 25;
    [SerializeField] float distanceToPunch = 0.25f;
    [SerializeField] float timeBetweenPunches = 1f;
    public Transform player;

    protected override void Update()
    {
        //RunToPlayer();

        if (player.gameObject.activeSelf)
        {
            CheckAndPerformPunch();
        }
        else
        {
            desiredMove.x *= -1f;
        }

        base.Update();
    }

    float lastPunchTime;
    private void CheckAndPerformPunch()
    {
        if (MathF.Abs(player.position.x - transform.position.x) < distanceToPunch)
        {
            desiredMove = Vector2.zero;
            if (Time.time - lastPunchTime > timeBetweenPunches)
            {
                PerformBomb();
                lastPunchTime = Time.time;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerGrenade"))
        {
            RoomController.Instance.EnemyDied(score);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
