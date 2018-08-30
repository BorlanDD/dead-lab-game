using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorScript : MonoBehaviour
{

    private Animator _animator;

    public bool locked;

    protected bool open;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        open = false;
		OnStart();
    }

    // Update is called once per frame
    void Update()
    {
		OnUpdate();
    }

	protected virtual void OnStart(){}
	protected virtual void OnUpdate(){}

    void OnTriggerEnter(Collider other)
    {
        if (open)
        {
            return;
        }
        if (other.gameObject.tag == "Player" && !locked)
        {
            open = true;
            OnOpen();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!open)
        {
            return;
        }
        if (other.gameObject.tag == "Player" && !locked)
        {
            open = false;
            OnClose();
        }
    }

    protected virtual void OnOpen()
    {
        _animator.SetTrigger("toOpen");
    }

    protected virtual void OnClose()
    {
        _animator.SetTrigger("toClose");
    }
}
