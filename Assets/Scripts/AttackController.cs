using UnityEngine;

public class AttackController : MonoBehaviour
{
    
    public float attackRange = 1.5f; // Attack range variable
    public Material idelStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    [Header("Combat Settings")]
    public float unitDamage = 10f;
    public GameObject hitEffect;
    [Header("Targeting")]
    public Transform targetToAttack;
    public float UnitDamage => unitDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack != other.transform)
        {
            targetToAttack = null;
        }
    }

    public void SetIdleMaterial()
    {
        GetComponent<Renderer>().material = idelStateMaterial;
    }
    public void SetFollowMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }
    public void SetAttackMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }


    private void OnDrawGizmos()
    {
        // Follow distance / Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f * 0.2f);

        // Attack distance / Area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Stop Attack distance / Area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange * 1.2f);
    }
}