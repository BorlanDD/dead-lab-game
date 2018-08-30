using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerForTask : Healer
{

	private UseHealerTask useHealerTask;

	public Kangaroo[] kangaroos;

    public override void Interract()
    {
        base.Interract();
        FindHealerTask fht = FindHealerTask.GetInstance();
		useHealerTask = UseHealerTask.GetInstance();

        if (fht != null && fht.started)
        {
            fht.addSubTask(useHealerTask);
        }

		for (int i = 0; i < kangaroos.Length; i++)
		{
			kangaroos[i].gameObject.SetActive(true);
		}
    }

	public override void OnUpdate()
	{
		if (useHealerTask != null && useHealerTask.started && !useHealerTask.completed && used)
		{
			useHealerTask.OnTaskFinish();
			FindHealerTask.GetInstance().OnTaskFinish();
		}
		base.OnUpdate();
	}
}
