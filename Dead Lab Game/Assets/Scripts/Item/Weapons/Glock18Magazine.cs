using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock18Magazine : Magazine {

	public override void OnAwake()
	{
		base.OnAwake();
		id = 11;
		usingWeaponId = 5;
		bulletCount = 18;
	}
}
