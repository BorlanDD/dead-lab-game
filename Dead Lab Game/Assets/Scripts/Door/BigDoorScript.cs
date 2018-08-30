using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigDoorScript : NoiseDoorScript {

	private GameObject link;
	protected override void OnStart() {
		base.OnStart();
		link = GetComponentInChildren<NavMeshLink>().transform.gameObject;
		link.SetActive(false);
	}

	protected override void OnOpen(){
		base.OnOpen();
		_source.clip = openDoor;
		_source.Play();
		link.SetActive(true);
	}

	protected override void OnClose(){
		base.OnClose();
		_source.clip = closeDoor;
		_source.Play();
		link.SetActive(false);
	}
}
