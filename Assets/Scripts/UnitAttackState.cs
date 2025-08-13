using System;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;
public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    public float stopAttackingDistance = 1.2f;
    public float attackRate = 2f; // Time between attacks in seconds
    private float attackTimer; // Timer to track attack rate

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial(); // Set the attack material when entering the state
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookatTarget();

            //Keep moving towards the target until close enough to attack
            agent.SetDestination(attackController.targetToAttack.position);

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate; // Reset the attack timer
            }
            else 
            { 
                attackTimer -= Time.deltaTime; // Decrease the timer
            }

                //Should unit still be attacking?
                float distanceToTarget = Vector3.Distance(animator.transform.position, attackController.targetToAttack.position);
            if (distanceToTarget > stopAttackingDistance || attackController.targetToAttack==null)
            {
                agent.SetDestination(animator.transform.position); // Stop moving when exiting the state
                animator.SetBool("isAttacking", false);
            }
        }
    }

    private void Attack() 
    {
        var damageToInflict = attackController.UnitDamage;

        //Actually attack the target
        var damageable = attackController.targetToAttack.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageToInflict);
        }

    }

    private void LookatTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.rotation.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0); // Lock rotation to Y-axis
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
