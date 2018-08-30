using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHealerTask : Task
{

    public DoorScript door;
    public static FindHealerTask findHealerTask;
    public static FindHealerTask GetInstance()
    {
        return findHealerTask;
    }

    public override void OnAwake()
    {
        findHealerTask = this;
    }

    public override void OnTaskStart()
    {
        description = "You are feeling bad. Find first-aid kit.";
        base.OnTaskStart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && FindGeneratorTask.GetInstance().completed && other.gameObject.tag == "Player")
        {
            OnTaskStart();
        }
    }

    public override void OnTaskFinish()
    {
        base.OnTaskFinish();
        door.locked = false;
        Destroy(gameObject);
    }

}
