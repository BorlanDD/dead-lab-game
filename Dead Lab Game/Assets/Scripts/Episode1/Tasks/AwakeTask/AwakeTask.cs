using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTask : Task
{

    private static AwakeTask awakeTask;

    public override void OnAwake()
	{
		awakeTask = this;
	}
    public override void OnTaskStart()
    {
        description = "Understand what happen.";
        base.OnTaskStart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.tag == "Player")
        {
            OnTaskStart();
        }
    }

    public static AwakeTask GetInstance()
    {
        return awakeTask;
    }

    public override void OnTaskFinish()
    {
        base.OnTaskFinish();
        Destroy(gameObject);
    }
}
