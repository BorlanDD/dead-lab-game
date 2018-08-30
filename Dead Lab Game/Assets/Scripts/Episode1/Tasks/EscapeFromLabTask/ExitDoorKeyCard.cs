using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorKeyCard : Item {

	public Kangaroo[] kangaroos;

	public override void Interract()
	{
		base.Interract();
		EscapeFromLabTask.GetInstance().OnTaskStart();

		for (int i = 0; i < kangaroos.Length; i++)
		{
			kangaroos[i].gameObject.SetActive(true);
		}
	}
}
