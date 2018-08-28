using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kangaroo : Enemy {

	private MeshRenderer meshRenderer;

	public SkinnedMeshRenderer head;
	public SkinnedMeshRenderer body;

	public Material defaultMat;
	public Material trancparencyMat;

	// Use this for initialization
	protected override void OnStart () {
		base.OnStart();
		health = 100;
		meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	protected override void OnUpdate () {
		base.OnUpdate();
		//Hide
		if (Input.GetKeyDown(KeyCode.H))
		{
			head.material = trancparencyMat;
			body.material = trancparencyMat;
		} 
		//Unhide
		else if (Input.GetKeyDown(KeyCode.U))
		{
			head.material = defaultMat;
			body.material = defaultMat;
		}
	}

	
}
