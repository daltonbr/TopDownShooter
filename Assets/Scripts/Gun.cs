using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenShots = 100f;
	public float muzzleVelocity = 35f;

    public Transform shell;
    public Transform shellEjection;

	float nextShotTime;

	public void Shoot()
	{
		if (Time.time > nextShotTime)
		{
			nextShotTime = Time.time + msBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
		}
	}

}
