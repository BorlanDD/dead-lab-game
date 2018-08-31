using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    public float returnTime;
    public float returnTimeLeft;

    private void Awake()
    {
        returnTime = 2;
    }

    public void Update()
    {
        if (returnTimeLeft >= returnTime)
        {
            returnTimeLeft = 0;
            ReturnToPull();
        }
        else
        {
            returnTimeLeft += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) {
            return;
        }
        Enemy enemy = other.transform.GetComponentInParent<Enemy>();
        other.transform.gameObject.GetComponent<Kangaroo>().shooted = true;
        enemy.health -= damage;
        //Debug.Log("Health: " + enemy.health);
        if (enemy.health <= 0)
        {
            enemy.Die();
        }

        ReturnToPull();
    }

    private void ReturnToPull()
    {
        BulletsPull.GetInstnace().ReturnBullet(this);
    }
}
