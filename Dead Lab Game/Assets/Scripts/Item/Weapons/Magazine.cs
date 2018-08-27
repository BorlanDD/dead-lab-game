using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item {

	public int bulletCount;
	public int usingWeaponId {get; protected set;}

	public override void OnAwake()
	{
		base.OnAwake();
		type = ItemType.Magazine;
	}

	public override void Interract()
	{
		base.Interract();
		Player player = Player.GetInstance();
		Weapon weapon = player.usingWeapon;

		if (weapon == null)
		{
			weapon.UpdateBullets();
		}
	}
}
