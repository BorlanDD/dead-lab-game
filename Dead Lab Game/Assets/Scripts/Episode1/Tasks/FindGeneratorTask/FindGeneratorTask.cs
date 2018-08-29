using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGeneratorTask : Task
{

    public static FindGeneratorTask findGeneratorTask;

    public static FindGeneratorTask GetInstance()
    {
        return findGeneratorTask;
    }

    public Generator generator;

    public override void OnAwake()
    {
        findGeneratorTask = this;
    }

    public override void OnTaskStart()
    {
        description = "Find and switch on energy generator.";
        base.OnTaskStart();

        AwakeTask awakeTask = AwakeTask.GetInstance();

        if (awakeTask != null)
        {
            awakeTask.OnTaskFinish();
        }
        generator.BrokeGenerator();
    }

    public override void OnTaskFinish()
    {
        base.OnTaskFinish();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.tag == "Player")
        {
            OnTaskStart();
        }
    }
}
