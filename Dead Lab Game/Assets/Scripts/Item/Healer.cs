using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Item {

	public Player player;
	public float healthCountPerPeriod;
	public int effectCount;
	public float effectPeriodTime;

	private int effectCountLeft;
	private float effectTimeLeft;

	protected bool used;

	public override void OnAwake()
	{
		id = 10;
		//used = false;
		effectPeriodTime = 1;
		effectCount = 25;
		healthCountPerPeriod = 0.02f;
	}

	public override void OnStart()
	{
		base.OnStart();
		player = Player.GetInstance();
		itemName = "Healer";
	}
	
	public override void OnUpdate()
	{
		base.OnUpdate();
		if (used)
		{
			
			if (effectTimeLeft >= effectPeriodTime && effectCountLeft < effectCount)
			{
				player.IsHealing = true;
				effectCountLeft++;
				if (player.Health < 1.0f)
				{
					player.Health += healthCountPerPeriod;
					if (player.Health >= 1.0f)
					{
						player.Health = 1.0f;
					}
				}

				if (effectCountLeft >= effectCount)
				{
					player.IsHealing = false;
					Destroy(gameObject);
				}
				effectTimeLeft = 0;
			}
			else 
			{
				effectTimeLeft += Time.deltaTime;
			}
		}
	}

	public void Use()
	{
		used = true;
	}
}
