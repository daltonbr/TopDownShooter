﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
	public float startingHealth;
	public float health { get; protected set; }
	protected bool dead;
    [HideInInspector]
    public LivingEntity targetEntity;

    public event System.Action OnDeath;

	public virtual void Start()
	{
		health = startingHealth;
	}

	public virtual void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
        // Later we will do some stuff here with the hit variable
        // Animations, particle effect, etc
        TakeDamage(damage);
	}

    public virtual void TakeDamage (float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
	public virtual void Die()
	{
		dead = true;

        if (OnDeath != null)
        {
            OnDeath();
        }
		GameObject.Destroy(this.gameObject);
	}

	public void RefillHealth ()
	{
		health = startingHealth;
	}

	public bool HasFullHealth()
	{
		return (health == startingHealth);
	}

    public float GetCurrentHealthPercent()
    {
        return health / startingHealth;
    }
			
}
