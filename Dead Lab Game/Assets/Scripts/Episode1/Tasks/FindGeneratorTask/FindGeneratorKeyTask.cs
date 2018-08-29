using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGeneratorKeyTask : Task {

	private static FindGeneratorKeyTask findGeneratorKeyTask;

	public static FindGeneratorKeyTask GetInstance(){
		return findGeneratorKeyTask;
	}

	public override void OnAwake()
	{
		findGeneratorKeyTask = this;
	}

	public override void OnTaskStart(){
		description = "Find generator battery.";
		type = Type.Subtask;
		Debug.Log("Find key start");
		base.OnTaskStart();
	}
	
}
