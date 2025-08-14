using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable
{
    public int health = 100;

    void Start()
    {
        UnitSelectionManager.instance.allUnits.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.instance.allUnits.Remove(gameObject);
    }

    public void TakeDamage(int damageToInflict)
    {
        health -= damageToInflict;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
