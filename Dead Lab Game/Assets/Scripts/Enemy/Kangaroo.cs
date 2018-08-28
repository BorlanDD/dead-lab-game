using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kangaroo : Enemy {

	public NavMeshAgent navMeshAgent;

	private MeshRenderer meshRenderer;
	public SkinnedMeshRenderer head;
	public SkinnedMeshRenderer body;

	public Material defaultMat;
	public Material trancparencyMat;


	public float lookRadius = 10f;
	private Player player;

	public FieldOfView eyes;

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;

	}

	// Use this for initialization
	protected override void OnStart () {
		base.OnStart();
		health = 100;
		meshRenderer = GetComponent<MeshRenderer>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		player = Player.GetInstance();
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


		if (eyes.IsSeePlayer()) {
			RunForPlayer();
		} 
		else 
		{
			Debug.Log("Потерял из виду!");
			if (lastPlayerPosition != Vector3.zero) {
				Debug.Log("Иду к последней точке");
				CheckLastPlayerPosition();
			}
		}
		
	}

	private Vector3 lastPlayerPosition;
	private void RunForPlayer() {
		navMeshAgent.SetDestination(player.transform.position);
		transform.LookAt(player.transform.position);
		lastPlayerPosition = player.transform.position;
	}

	private void CheckLastPlayerPosition() {
		if (transform.position != lastPlayerPosition) {
			navMeshAgent.SetDestination(lastPlayerPosition);
			transform.LookAt(lastPlayerPosition);
		}
		else
		{
			lastPlayerPosition = Vector3.zero;
		}
		
		
	}

	private void Patrol() {

	}

	
}
