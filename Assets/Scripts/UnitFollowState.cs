using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    public float attackingDistance = 1.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        agent = animator.GetComponent<NavMeshAgent>();
        attackController.SetFollowMaterial();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Exit if no target
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
            return;
        }

        // Exit if commanded to move
        if (animator.GetComponent<UnitMovement>().isCommandedToMove)
        {
            animator.SetBool("isFollowing", false);
            return;
        }

        // Calculate distance once per frame
        float distanceToTarget = Vector3.Distance(animator.transform.position,
                                               attackController.targetToAttack.position);

        // PRIORITY 1: Attack if in range
        if (distanceToTarget <= attackingDistance)
        {
            agent.SetDestination(animator.transform.position); // Stop moving
            animator.SetBool("isAttacking", true);
            animator.SetBool("isFollowing", false);
            return;
        }

        // PRIORITY 2: Continue following if out of range
        agent.SetDestination(attackController.targetToAttack.position);
        animator.transform.LookAt(attackController.targetToAttack);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position); // Stop moving when exiting
    }
}