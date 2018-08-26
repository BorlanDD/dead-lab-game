using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour {

	#region SINGLETON PATTERN
    private static WeaponUI instance;
    public static WeaponUI GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion

	
	public Image ak47;
	public Image glock;
	public Text bulletCountUI;


	#region RIFLE
	public Image rifle;
	public GameObject rifleSingleMode;
	public GameObject rifleBurstMode;
	public GameObject rifleAutomaticMode;
	public GameObject rifleLockedMode;
	public Text rifleBulletCounts;
	#endregion


	#region RIFLE
	public Image gun;
	public GameObject gunSingleMode;
	public GameObject gunBurstMode;
	public GameObject gunLockedMode;
	public Text gunBulletCounts;
	#endregion

	private int maxBulletCount;
	public void UpdateSprites(Weapon weapon) {
		if (weapon == null) {
			gun.enabled = false;
			gunSingleMode.SetActive(false);
			gunBurstMode.SetActive(false);
			gunLockedMode.SetActive(false);
			gunBulletCounts.enabled = false;

			rifle.enabled = false;
			rifleSingleMode.SetActive(false);
			rifleBurstMode.SetActive(false);
			rifleAutomaticMode.SetActive(false);
			rifleLockedMode.SetActive(false);
			rifleBulletCounts.enabled = false;

			return;
		}

		if (weapon.weaponType == Weapon.Type.Automat) {
			rifle.enabled = true;
			if (weapon.currentShootingMode == Weapon.ShootingMode.Single) {
				rifleSingleMode.SetActive(true);
				rifleBurstMode.SetActive(false);
				rifleAutomaticMode.SetActive(false);
				rifleLockedMode.SetActive(false);
			} else if (weapon.currentShootingMode == Weapon.ShootingMode.Locked) {
				rifleSingleMode.SetActive(false);
				rifleBurstMode.SetActive(false);
				rifleAutomaticMode.SetActive(false);
				rifleLockedMode.SetActive(true);
			} else if (weapon.currentShootingMode == Weapon.ShootingMode.Burst) {
				rifleSingleMode.SetActive(false);
				rifleBurstMode.SetActive(true);
				rifleAutomaticMode.SetActive(false);
				rifleLockedMode.SetActive(false);
			} else if (weapon.currentShootingMode == Weapon.ShootingMode.Automatic) {
				rifleSingleMode.SetActive(false);
				rifleBurstMode.SetActive(false);
				rifleAutomaticMode.SetActive(true);
				rifleLockedMode.SetActive(false);
			}

			gun.enabled = false;
			gunSingleMode.SetActive(false);
			gunBurstMode.SetActive(false);
			gunLockedMode.SetActive(false);
			gunBulletCounts.enabled = false;

			rifleBulletCounts.enabled = true;
			rifleBulletCounts.text = "" + weapon.bulletCounts  + "/" + weapon.MaxbulletCounts;
		} 
		else if (weapon.weaponType == Weapon.Type.Pistol)
		{
			gun.enabled = true;

			if (weapon.currentShootingMode == Weapon.ShootingMode.Single) {
				gunSingleMode.SetActive(true);
				gunBurstMode.SetActive(false);
				gunLockedMode.SetActive(false);
			} else if (weapon.currentShootingMode == Weapon.ShootingMode.Locked) {
				gunSingleMode.SetActive(false);
				gunBurstMode.SetActive(false);
				gunLockedMode.SetActive(true);
			} else if (weapon.currentShootingMode == Weapon.ShootingMode.Burst) {
				gunSingleMode.SetActive(false);
				gunBurstMode.SetActive(true);
				gunLockedMode.SetActive(false);
			} 

			rifle.enabled = false;
			rifleSingleMode.SetActive(false);
			rifleBurstMode.SetActive(false);
			rifleAutomaticMode.SetActive(false);
			rifleLockedMode.SetActive(false);
			rifleBulletCounts.enabled = false;

			gunBulletCounts.enabled = true;
			gunBulletCounts.text = "" + weapon.bulletCounts  + "/" + weapon.MaxbulletCounts;
		}
	}
}
