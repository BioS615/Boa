using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    public float collisionDelay;
    public int attackPower;
    public bool hasStruck;

    void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("Collision: {0}", other.name);
        if (other.GetComponent<TestDummy>() && hasStruck == false)
        {
            hasStruck = true;
            other.GetComponent<TestDummy>().TakeDamage(attackPower);

        }
        var direction = (this.GetComponent<Rigidbody>().position - other.transform.position).normalized;
        other.GetComponent<Rigidbody>().AddForce(-direction * 2f, ForceMode.Impulse);
    }
}
