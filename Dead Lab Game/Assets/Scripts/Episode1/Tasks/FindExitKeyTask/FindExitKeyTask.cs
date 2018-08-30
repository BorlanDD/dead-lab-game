using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindExitKeyTask : Task
{

    private static FindExitKeyTask findExitKeyTask;
	public DoorScript door;
    public static FindExitKeyTask GetInstance()
    {
        return findExitKeyTask;
    }

    public override void OnAwake()
    {
        base.OnAwake();
        findExitKeyTask = this;
    }
    public override void OnTaskStart()
    {
		description = "Go to warehouse and find exit key.";
		base.OnTaskStart();
		door.locked = false;
    }

    public override void OnTaskFinish()
    {
		base.OnTaskFinish();
		Destroy(gameObject);
    }
}
