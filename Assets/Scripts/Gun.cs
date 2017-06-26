using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Gun : MonoBehaviour
{

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
    private GameUI gameUI;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float muzzleVelocity = 35f;
    public int burstCount = 3;
    public int projectilesPerMag;
    public float reloadTime = .3f;
    public int initialMagazines = 2;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3f, 5f);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;
    MuzzleFlash muzzleFlash;


    float nextShotTime;
    public int currentMagazines { get; private set; }

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    public int projectilesRemainingInMag { get; private set; }
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    private void Start()
    {
        //Assert.IsNotNull(gameUI, "Gun::Start - Can't find gameUI");
        Assert.IsNotNull(shootAudio, "Gun::Start - Can't find shootAudio");
        Assert.IsNotNull(reloadAudio, "Gun::Start - Can't find reloadAudio");
        muzzleFlash = GetComponent<MuzzleFlash>();
        Assert.IsNotNull(muzzleFlash, "Gun::Start - Can't find MuzzleFlash Component!");
        shotsRemainingInBurst = burstCount;
        if (fireMode == FireMode.Burst && burstCount <= 0)
        {
            Debug.LogError("Error! Gun.cs: burstCount must be a positive value (when in Burst mode)");
        }

        projectilesRemainingInMag = projectilesPerMag;
        currentMagazines = initialMagazines;
        tryUpdateAmmoUI();
    }

    private void LateUpdate()
    {
        // Animate the recoil (smoothing back)
        this.transform.localPosition = Vector3.SmoothDamp(this.transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        this.transform.localEulerAngles = Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMag == 0 && !isOutOfBullets())
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0 && !isOutOfBullets())
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
                if (projectilesRemainingInMag == 0) { break; }
                projectilesRemainingInMag--;
                tryUpdateAmmoUI();
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed(muzzleVelocity);
            }
            // Shell ejection
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            // Recoil
            this.transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            // shoot Audio
            AudioManager.instance.PlaySound(shootAudio, this.transform.position);
        }
    }

    public void Reload()
    {
        if (isOutOfBullets())
        {
            Debug.Log("Out of magazines! Can't reload!");
            return;
        }

        if (!isReloading && projectilesRemainingInMag != projectilesPerMag)
        {
            // Update currentMagazines and UI's
            currentMagazines--;
            StartCoroutine(AnimateReload());
            
            
            // Reload Audio
            AudioManager.instance.PlaySound(reloadAudio, this.transform.position);
        }
    }

    public void registerGameUI(GameUI gameUI)
    {
        this.gameUI = gameUI;
    }

    private void tryUpdateAmmoUI()
    {
        if (gameUI)
        {
            gameUI.ClipCountUI.text = currentMagazines.ToString("D2");
            gameUI.AmmoCountUI.text = projectilesRemainingInMag.ToString("D2");
        }
    }

    public bool isOutOfBullets()
    {
        return (currentMagazines == 0 && projectilesRemainingInMag == 0);
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = this.transform.localEulerAngles;
        float maxReloadAngle = 30f;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            this.transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        // Reloading
        projectilesRemainingInMag = projectilesPerMag;
        tryUpdateAmmoUI();
    }

    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            this.transform.LookAt(aimPoint);
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
