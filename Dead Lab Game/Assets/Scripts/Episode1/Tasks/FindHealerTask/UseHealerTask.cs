using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHealerTask : Task
{

    private static UseHealerTask useHealerTask;

    public static UseHealerTask GetInstance()
    {
        return useHealerTask;
    }

    public override void OnAwake()
    {
        useHealerTask = this;
    }

    public override void OnTaskStart()
    {
        description = "Use healer.";
        type = Type.Subtask;

        base.OnTaskStart();
    }

    public override void OnTaskFinish()
    {
        base.OnTaskFinish();
    }
}
