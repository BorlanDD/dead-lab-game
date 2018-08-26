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

	private int maxBulletCount;
	public void UpdateSprite(Weapon weapon) {
		if (weapon.weaponType == Weapon.Type.Automat) {
			ak47.enabled = true;
			glock.enabled = false;
			//bulletCountUI.text = weapon.bulletCounts + "/" + weapon.MaxbulletCounts;
		} 
		else if (weapon.weaponType == Weapon.Type.Pistol)
		{
			ak47.enabled = false;
			glock.enabled = true;
			//bulletCountUI.text = weapon.bulletCounts + "/" + weapon.MaxbulletCounts;
		}
		maxBulletCount = weapon.MaxbulletCounts;
	}

	public void BulletCountUpdate(int count) {
		bulletCountUI.text = "" + count  + "/" + maxBulletCount;
	}
}
