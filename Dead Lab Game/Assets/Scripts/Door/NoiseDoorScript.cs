using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NoiseDoorScript : DoorScript
{


    [SerializeField] protected AudioClip openDoor;
    [SerializeField] protected AudioClip closeDoor;
    

    protected AudioSource _source;


    protected override void OnStart()
    {
        _source = GetComponent<AudioSource>();
    }
}
