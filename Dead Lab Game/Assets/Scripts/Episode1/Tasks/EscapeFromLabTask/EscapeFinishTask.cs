using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFinishTask : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		EscapeFromLabTask escapeFinishTask = EscapeFromLabTask.GetInstance();
		if (escapeFinishTask != null && !escapeFinishTask.completed)
		{
			escapeFinishTask.OnTaskFinish();
		}
	}
}
