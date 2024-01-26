using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    public int health = 100;
    public int armor = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.LogFormat("New health: {0}", health);
        if (health < 1) { Die(); }
    }

    public void Die()
    {
        transform.DOScale(Vector3.zero, 1f);
    }

}
