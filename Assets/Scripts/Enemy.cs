using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

	NavMeshAgent pathfinder;
	Transform target;

	public override void Start () {
		base.Start();
		pathfinder = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Player").transform;

		StartCoroutine(UpdatePath ());
	}

	IEnumerator UpdatePath()
	{
		float refreshRate = 0.25f;

		while (target != null)
		{
			Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
			if(!dead)
			{
				pathfinder.SetDestination (targetPosition);
			}

			yield return new WaitForSeconds(refreshRate);
		}
	}
}
