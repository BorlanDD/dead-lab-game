using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseKeyCard : Item {

	public Kangaroo[] kangaroos;

	public override void Interract()
	{
		base.Interract();
		FindExitKeyTask fekt = FindExitKeyTask.GetInstance();
		fekt.OnTaskStart();

		for (int i = 0; i < kangaroos.Length; i++)
		{
			kangaroos[i].gameObject.SetActive(true);
		}
	}
}
