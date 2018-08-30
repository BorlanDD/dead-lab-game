using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFromLabTask : Task {

	public DoorScript door;

	private static EscapeFromLabTask escapeFromLabTask;

	public static EscapeFromLabTask GetInstance()
	{
		return escapeFromLabTask;
	}

	public override void OnAwake()
	{
		base.OnAwake();
		escapeFromLabTask = this;
	}

	public override void OnTaskStart()
	{
		description = "Escape from labaratory.";
		base.OnTaskStart();
		door.locked = false;
		FindExitKeyTask.GetInstance().OnTaskFinish();
	}

	public override void OnTaskFinish()
	{
		base.OnTaskFinish();
	}
}
