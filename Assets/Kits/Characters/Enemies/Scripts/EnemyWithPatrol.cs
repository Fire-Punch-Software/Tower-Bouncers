using System;
using UnityEngine;

public class EnemyWithPatrol : BaseEnemy
{
    [Header("Patrol")]
    [SerializeField] private float distanceTolerance = 0.1f;
    [SerializeField] private Transform[] waypoints;
    private int index = 0;
    private Vector3 actualDestination;

    protected override void Awake()
    {
        base.Awake();

        // VALIDACIÓN waypoints
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("EnemyWithPatrol: Asigna waypoints en Inspector!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        SetNewDestination();
        RoomController.Instance.RegisterEnemy();
    }

     protected override void Update()
    {
        desiredMove = (actualDestination - transform.position).normalized;

        // Si llegó, cambiar destino
        if (Vector3.Distance(transform.position, actualDestination) < distanceTolerance)
        {
            SetNewDestination();
        }

        base.Update();

        rb2D.linearVelocityY = desiredMove.y * actualWalkSpeed;
    }

    private void SetNewDestination()
    {
        index = (index + 1) % waypoints.Length;
        actualDestination = waypoints[index].position;
    }
}
