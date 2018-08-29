using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle; 


	public LayerMask targetMask;
	public LayerMask obstacleMask;
	[HideInInspector]
	//public List<Transform> visibleTargets = new List<Transform>();

	/* private Kangaroo _kangaroo;

	void Awake() {
		_kangaroo = GetComponentInParent<Kangaroo>();
	} */

	void Update() {
		
	}

	public List<Kangaroo> FindVisibleTargets() {
		List<Kangaroo> visibleTargets = new List<Kangaroo>();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
				float distToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
					visibleTargets.Add(target.gameObject.GetComponent<Kangaroo>());
					//_kangaroo.enabled = true;
				}
			}
		}

		return visibleTargets;
	}

	public bool IsSeePlayer() {
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
				float distToTarget = Vector3.Distance(transform.position, target.position);

				if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
					return true;
				}
			}
		}
		return false;
	}
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}
