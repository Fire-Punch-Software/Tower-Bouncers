using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField]  float radius = 5f;
    [SerializeField]  float checkFrequency = 5f;

    float lastCheckTime;
    Collider2D[] colliders;
    void Update()
    {
        if ((Time.time - lastCheckTime) > (1f / checkFrequency))
        {
            lastCheckTime = Time.time;

            colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        }
    }

    public bool isPlayerInSight()
    {
        bool isPlayerDetected = false;

        if (!isPlayerDetected && colliders != null && colliders.Length != 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    isPlayerDetected = true;
                }
            }
        }

        return isPlayerDetected;
    }
}
