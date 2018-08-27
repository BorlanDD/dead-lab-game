using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUtils
{

    public static Weapon GetWeaponBySlot(int slot)
    {
        Weapon weapon;
        IList<Item> weapons = Player.GetInstance().inventory.GetItemByType(ItemType.Weapon);
        for (int i = 0; i < weapons.Count; i++)
        {
			weapon = (Weapon)weapons[i];
            if (weapon.slot == slot)
            {
                Player.GetInstance().inventory.GetItem(weapon.id);
				return weapon;
            }
        }

        return null;
    }

    public static IList<Magazine> BUlletsLeft(Weapon weapon)
    {
        Inventory inventory =  Player.GetInstance().inventory;
        IList<Item> magazines = inventory.GetItemByType(ItemType.Magazine);
        IList<Magazine> needMagazines = new List<Magazine>();

        int count = 0;
        Magazine magazine;
        for (int i = 0; i < magazines.Count; i++)
        {
            magazine = (Magazine)magazines[i];
            if (magazine.usingWeaponId == weapon.id)
            {
                needMagazines.Add(magazine);
            }
        }
        return needMagazines;
    }
}
