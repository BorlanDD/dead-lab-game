using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPoint : MonoBehaviour {

	private static GreenPoint greenPoint;
	public static GreenPoint GetInstance()
	{
		return greenPoint;
	}
	void Awake()
	{
		greenPoint  = this;
	}
}
