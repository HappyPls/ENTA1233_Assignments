using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AIBulletManager : BaseBulletManager
{
    [Header("Bullet Spawn")]
    [SerializeField] private Transform BulletSpawnPoint;

    [Header("Firing Settings")]
    [SerializeField] private float FireRate = 1.5f;
    [SerializeField] private float DetectionRadius = 10f;
    private float CooldownTimer = 0f;
    private Transform currentTarget;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (currentTarget == null || BulletSpawnPoint == null) 
            return;

        if (!agent.isStopped)
            return;

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        if (!Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit hit, DetectionRadius) ||
            hit.transform != currentTarget)
        {
            return; // Player no longer visible
        }

        CooldownTimer -= Time.deltaTime;

        if (CooldownTimer <= 0f)
        {
            Vector3 aimPoint = currentTarget.position + Vector3.up * 1.2f; // Adjust Y as needed
            Vector3 dirToTarget = (aimPoint - BulletSpawnPoint.position).normalized;

            SpawnPhysicsBullet(BulletSpawnPoint);
            CooldownTimer = FireRate;
        }
    }

    public void EngageTarget(Transform target)
    {
        currentTarget = target;
    }

    public void Disengage()
    {
        currentTarget = null;
    }
}
