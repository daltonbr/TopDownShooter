using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    public enum State {Idle, Chasing, Attacking};
    State currentState;

    public GameObject deathEffect;

	NavMeshAgent pathfinder;
	Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1f;
    float damage = 1f;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    public override void Start () {
		base.Start();
		pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = this.GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

		    StartCoroutine(UpdatePath ());
        }
	}

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        // Instantiating death effect
        if (damage >= health)
        {
            Destroy(Instantiate(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, 2);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    private void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = (target.position - this.transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = this.transform.position;
        Vector3 dirToTarget = (target.position - this.transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);
        
        float attackSpeed = 3f;
        float percent = 0f;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >=.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            this.transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            // skip a frame
            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
	{
		float refreshRate = 0.25f;

		while (hasTarget)
		{
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - this.transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
			    if(!dead)
			    {
				    pathfinder.SetDestination (targetPosition);
			    }
            }

			yield return new WaitForSeconds(refreshRate);
		}
	}
}
