using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47Magazine : Magazine {

	public override void OnAwake()
	{
		base.OnAwake();
		id = 12;
		usingWeaponId = 5;
		bulletCount = 30;
	}
}
