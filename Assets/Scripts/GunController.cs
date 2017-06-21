using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun[] allGuns;
	Gun equippedGun;


	public void EquipGun(Gun gunToEquip)
	{
		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas)
        {
            GameUI gameUI = canvas.GetComponent<GameUI>();
            equippedGun.registerGameUI(gameUI);
        }
        else
        {
            Debug.Log("[GunController] Canvas / GameUI not found! Can't register gun in GameUI!");
        }
        
	}
		

    public void EquipGun(int weaponIndex)
    {
        EquipGun(allGuns[weaponIndex]);
    }


	public void OnTriggerHold()
	{
		if (equippedGun != null)
		{
			equippedGun.OnTriggerHold();
		}
	}

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public float GunHeight
    {
        get
        {
            return weaponHold.position.y;
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (equippedGun != null)
        {
            equippedGun.Aim(aimPoint);
        }
    }

    public void Reload()
    {
        if (equippedGun != null)
        {
            equippedGun.Reload();
        }
    }

}
