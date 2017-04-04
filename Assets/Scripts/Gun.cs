using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Gun : MonoBehaviour
{
    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float muzzleVelocity = 35f;
    public int burstCount = 3;

    public Transform shell;
    public Transform shellEjection;
    MuzzleFlash muzzleFlash;

    float nextShotTime;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    private void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        Assert.IsNotNull(muzzleFlash, "Gun::Start - Can't find MuzzleFlash Component!");
        shotsRemainingInBurst = burstCount;
        if (fireMode == FireMode.Burst && burstCount <= 0)
        {
            Debug.LogError("Error! Gun.cs: burstCount must be a positive value (when in Burst mode)");
        }
    }

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            // Handling Fire Modes
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0) { return; }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot) { return; }
            }

            // Spawning shot(s)
            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }
            // Shell ejection
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;

    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
