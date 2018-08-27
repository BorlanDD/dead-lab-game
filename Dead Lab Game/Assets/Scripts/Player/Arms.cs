using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{

    public Player player;

    public Transform glock18MagPos;
    public Transform ak47MagPos;

    public Transform healerPos;

    public void TakeOffWeapon()
    {
        player.TakeOffWeapon();
    }

    public void SetWeapon()
    {
        player.SetWeapon();
    }

    public void SettedWeapon()
    {
        if (player.usingWeapon == null)
        {
            return;
        }
        player.usingWeapon.Setted();
    }

    public void ReloadedWeapon()
    {
        player.ReloadedWeapon();
    }

    public void TakeMag()
    {
        Weapon weapon = player.usingWeapon;
        if (weapon != null)
        {
            if (weapon.itemName.Equals(Glock18.WEAPON_NAME))
            {
                weapon.magazin.transform.SetParent(glock18MagPos);
                weapon.magazin.transform.localPosition = new Vector3(0, 0, 0);
            }
            else if (weapon.itemName.Equals(AK47.WEAPON_NAME))
            {
                weapon.magazin.transform.SetParent(ak47MagPos);
                weapon.magazin.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    public void SetHealer()
    {
        player.healer.transform.SetParent(healerPos);
        player.healer.transform.localPosition = new Vector3(0, 0, 0);
        player.healer.transform.localRotation = new Quaternion(0, 0, 0, 0);
        player.healer.gameObject.SetActive(true);
    }

    public void UsedHealer()
    {
        player.healer.Use();
        Debug.Break();
    }

    public void EquipPreviousWeapon()
    {
        if (player.healer != null)
        {
            player.healer.gameObject.GetComponent<MeshRenderer>().enabled = false;
            player.healer = null;
        }
        player.EquipWeapon(player.beforeHealerWeapon);
    }
}
