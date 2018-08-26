﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : Weapon
{

    public static string WEAPON_NAME = "AK-47";
    public override void OnAwake()
    {
        base.OnAwake();
        itemName = "AK-47";
        weaponType = Type.Automat;

        availableShootingModes.Add(ShootingMode.Single);
        availableShootingModes.Add(ShootingMode.Burst);
        availableShootingModes.Add(ShootingMode.Automatic);
        dissipateAutomaticStartThrough = 0.0005f;

        afterSingleDelay = 0.2f;
		afterAutomaticDelay = 0.075f;

        burstBulletCount = 3;
        bulletSpeed = 75;
        burstModeDelay = 0.08f;
        afterBurstDelay = 0.6f;

        slot = 2;
        MaxbulletCounts = 30;
        bulletCounts = 30;
        damage = 30;
    }
}
