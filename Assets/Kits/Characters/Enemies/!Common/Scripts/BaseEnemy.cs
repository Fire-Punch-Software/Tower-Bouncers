using System;
using UnityEngine;

public class BaseEnemy : MovementController
{
    [Header("Enemy Settings")]
    [SerializeField] float distanceToPunch = 0.25f;
    [SerializeField] float timeBetweenPunches = 1f;
    public Transform player;

    float lastPunchTime;
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

    private void CheckAndPerformPunch()
    {
        if (MathF.Abs(player.position.x - transform.position.x) < distanceToPunch)
        {
            desiredMove = Vector2.zero;
            if (Time.time - lastPunchTime > timeBetweenPunches)
            {
                PerformPunch();
                lastPunchTime = Time.time;
            }
        }
    }
}
