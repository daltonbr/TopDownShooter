using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    public enum State {Idle, Chasing, Attacking};
    State currentState;

	NavMeshAgent pathfinder;
	Transform target;
    Material skinMaterial;

    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1f;
    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    public override void Start () {
		base.Start();
		pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        currentState = State.Chasing;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        myCollisionRadius = this.GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

		StartCoroutine(UpdatePath ());
	}

    private void Update()
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

        while (percent <= 1)
        {
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

		while (target != null)
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
