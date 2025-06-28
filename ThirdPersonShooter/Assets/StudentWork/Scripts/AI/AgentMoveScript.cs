using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveScript : MonoBehaviour
{
    [Header("Navigation")]
    private Transform[] PatrolPoints;
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private float StoppingDistance = 2f;

    [Header("Detection Settings")]
    [SerializeField] private float DetectionRadius = 10f;
    [SerializeField] private float DetectionAngle = 60f;
    [SerializeField] private LayerMask DetectionLayer;
    [SerializeField] private float MemoryDuration = 5f;
    [SerializeField] private float InvestigateThreshold = 1.5f;
    [SerializeField] private float RotationSpeed = 10f;

    [SerializeField] private AIBulletManager BulletManager;

    private int currentPatrolIndex = 0;
    private Transform Player;
    private float memoryTimer = 0f;
    private bool hasSeenPlayer = false;
    private Vector3 lastKnownPlayerPosition;
    private bool playerInSight;

    private void Start()
    {
        AssignRandomPatrolPoints();
    }

    void Update()
    {
        Transform detectedPlayer = DetectPlayer();

        if (detectedPlayer != null)
        {
            Player = detectedPlayer;

            if (!hasSeenPlayer)
            {
                hasSeenPlayer = true;
                BulletManager.EngageTarget(detectedPlayer);
            }

            memoryTimer = MemoryDuration;
            lastKnownPlayerPosition = Player.position;
        }

        if (hasSeenPlayer)
        {
            PlayerEngagement();
        }
        else
        {
            if (NavMeshAgent != null && NavMeshAgent.enabled && NavMeshAgent.isOnNavMesh)
            {
                if (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance < 0.5f)
                {
                    NavMeshAgent.isStopped = false;
                    PatrolToNextPoint();
                }
            }
        }
    }

    private void PatrolToNextPoint()
    {
        if (PatrolPoints.Length == 0)
        {
            return;
        }
        NavMeshAgent.destination = PatrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % PatrolPoints.Length;
    }

    private Transform DetectPlayer()
    {
        playerInSight = false;
        Collider[]hits = Physics.OverlapSphere(transform.position, DetectionRadius, DetectionLayer);

        foreach (var hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < DetectionAngle /2f)
            {
                if(Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit rayHit, DetectionRadius))
                {
                    if (rayHit.transform == hit.transform)
                    {
                        playerInSight = true;
                        return hit.transform;
                    }
                }
            }
        }
        return null;
    }

    private void PlayerEngagement()
    {
        

        float distanceToLastKnown = Vector3.Distance(transform.position, lastKnownPlayerPosition);

        if (memoryTimer > 0f)
        {
            memoryTimer -= Time.deltaTime;
        }

        if (distanceToLastKnown > StoppingDistance)
        {
            if (NavMeshAgent != null && NavMeshAgent.enabled && NavMeshAgent.isOnNavMesh)
            {
                NavMeshAgent.isStopped = false;
                NavMeshAgent.SetDestination(lastKnownPlayerPosition);
                BulletManager.Disengage();
            }
        }
        else
        {
            NavMeshAgent.isStopped = true;

            Vector3 direction = (lastKnownPlayerPosition - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            }

            if (Player != null && playerInSight)
            {
                BulletManager.EngageTarget(Player);
            }
            else
            {
                BulletManager.Disengage();
            }

            if (memoryTimer <= 0f)
            {
                hasSeenPlayer = false;
                Player = null;
                NavMeshAgent.isStopped = false;
                BulletManager.Disengage();
                PatrolToNextPoint();
            }
        }
    }

    private void AssignRandomPatrolPoints()
    {
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

        if (allPoints.Length < 3)
        {
            Debug.LogWarning("Not enough patrol points in the scene!");
            return;
        }
        List<GameObject> shuffled = new List<GameObject>(allPoints);
        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject temp = shuffled[i];
            int randomIndex = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        PatrolPoints = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            PatrolPoints[i] = shuffled[i].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);

        // Draw cone field of view
        Vector3 forward = transform.forward * DetectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -DetectionAngle / 2f, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, DetectionAngle / 2f, 0);
        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }
}
