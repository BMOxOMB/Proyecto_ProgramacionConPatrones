using RTSGame.AI;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AttackController attackController;
    private Transform currentTarget;

    [Header("Combat Settings")]
    public float stopAttackingDistance = 1.8f;
    public float attackRate = 2f;

    private float attackTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        currentTarget = attackController.targetToAttack;

        attackController.SetAttackMaterial();
        attackTimer = 0f;
        agent.isStopped = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if target is valid
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            ExitAttackState(animator);
            return;
        }

        // Check distance to target
        float distance = Vector3.Distance(animator.transform.position, currentTarget.position);
        if (distance > stopAttackingDistance)
        {
            ExitAttackState(animator, shouldFollow: true);
            return;
        }

        // Handle attack cooldown
        if (attackTimer <= 0f)
        {
            ExecuteAttack();
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void ExecuteAttack()
    {
        if (currentTarget == null) return;

        // Get the Health component directly
        Health health = currentTarget.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(attackController.UnitDamage);
            Debug.Log($"Dealt {attackController.UnitDamage} damage to {currentTarget.name}. Remaining health: {health.CurrentHP}");

            // Visual feedback
            if (attackController.hitEffect != null)
            {
                Instantiate(attackController.hitEffect, currentTarget.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning($"No Health component found on {currentTarget.name}");
        }
    }

    private void ExitAttackState(Animator animator, bool shouldFollow = false)
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isFollowing", shouldFollow);
        agent.isStopped = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false;
    }
}