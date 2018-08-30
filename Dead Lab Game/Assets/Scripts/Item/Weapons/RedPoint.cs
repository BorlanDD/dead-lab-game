using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPoint : MonoBehaviour {

	private static RedPoint redPoint;
	public static RedPoint GetInstance()
	{
		return redPoint;
	}
	void Awake()
	{
		redPoint = this;
	}
}
