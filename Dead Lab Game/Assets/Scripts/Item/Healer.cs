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

	public bool IsUsed() {
		return used;
	}

	public override void OnAwake()
	{
		id = 10;
		//used = false;
		effectPeriodTime = 1;
		effectCount = 25;
		healthCountPerPeriod = 0.02f;
		type = ItemType.Healer;
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

				HealerUI.GetInstance().healerLevel.fillAmount = (effectCount - effectCountLeft) * 1.0f / effectCount;
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
