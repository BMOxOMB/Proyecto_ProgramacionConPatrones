using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 100;

    public void TakeDamage(int damageToInflict)
    {
        health -= damageToInflict;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
