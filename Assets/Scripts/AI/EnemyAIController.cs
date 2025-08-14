using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RTSGame.AI
{
    /// <summary>
    /// Basic enemy AI using a simple FSM: Idle, Chase, Attack.
    /// Prioritises the closest player unit, then the town center, then other buildings.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAIController : MonoBehaviour
    {
        private enum State { Idle, Chase, Attack }

        [Header("Targeting")]
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackRadius = 2f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private Team team = Team.Enemy;
        [SerializeField] private float repathTime = 2f;

        [Header("Attack")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attackInterval = 1f;

        private readonly Collider[] detectionBuffer = new Collider[30];
        private NavMeshAgent agent;
        private State state = State.Idle;
        private Targetable currentTarget;
        private float lastAttackTime;
        private float pathFailTimer;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle:
                    HandleIdle();
                    break;
                case State.Chase:
                    HandleChase();
                    break;
                case State.Attack:
                    HandleAttack();
                    break;
            }
        }

        private void HandleIdle()
        {
            AcquireTarget();
            if (currentTarget != null)
            {
                state = State.Chase;
            }
        }

        private void HandleChase()
        {
            if (currentTarget == null || !currentTarget.IsAlive)
            {
                state = State.Idle;
                return;
            }

            float sqrDist = (currentTarget.TargetTransform.position - transform.position).sqrMagnitude;
            if (sqrDist > detectionRadius * detectionRadius)
            {
                currentTarget = null;
                state = State.Idle;
                return;
            }

            if (sqrDist <= attackRadius * attackRadius)
            {
                agent.ResetPath();
                state = State.Attack;
                return;
            }

            if (!agent.hasPath || agent.destination != currentTarget.TargetTransform.position)
            {
                agent.SetDestination(currentTarget.TargetTransform.position);
                pathFailTimer = 0f;
            }

            if (agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                pathFailTimer += Time.deltaTime;
                if (pathFailTimer >= repathTime)
                {
                    AcquireTarget();
                    pathFailTimer = 0f;
                }
            }
        }

        private void HandleAttack()
        {
            if (currentTarget == null || !currentTarget.IsAlive)
            {
                state = State.Idle;
                return;
            }

            float sqrDist = (currentTarget.TargetTransform.position - transform.position).sqrMagnitude;
            if (sqrDist > attackRadius * attackRadius)
            {
                state = State.Chase;
                return;
            }

            agent.ResetPath();
            transform.LookAt(currentTarget.TargetTransform.position);

            if (Time.time - lastAttackTime >= attackInterval)
            {
                lastAttackTime = Time.time;
                currentTarget.Health.ApplyDamage(damage);
                if (!currentTarget.IsAlive)
                {
                    currentTarget = null;
                    state = State.Idle;
                }
            }
        }

        private void AcquireTarget()
        {
            currentTarget = FindBestTarget();
            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.TargetTransform.position);
                state = State.Chase;
            }
        }

        private Targetable FindBestTarget()
        {
            int hits = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, detectionBuffer, targetMask);
            Targetable nearestUnit = null;
            float nearestUnitDist = float.MaxValue;
            Targetable townCenter = null;
            Targetable building = null;

            for (int i = 0; i < hits; i++)
            {
                Targetable t = detectionBuffer[i].GetComponent<Targetable>();
                if (t == null || !TeamHelper.IsEnemy(team, t.Team) || !t.IsAlive)
                    continue;

                float dist = (t.TargetTransform.position - transform.position).sqrMagnitude;
                if (!t.IsBuilding)
                {
                    if (dist < nearestUnitDist)
                    {
                        nearestUnitDist = dist;
                        nearestUnit = t;
                    }
                }
                else
                {
                    if (t.IsTownCenter)
                    {
                        townCenter = t;
                    }
                    else if (building == null)
                    {
                        building = t;
                    }
                }
            }

            return nearestUnit ?? townCenter ?? building;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}
