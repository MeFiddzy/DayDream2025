using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupFrogs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<MovementManager>() == null)
            return;
        GameObject.Find("slime").GetComponent<SlimeEnemy>().m_speed += 2f;
        GameObject.Find("slime (1)").GetComponent<SlimeEnemy>().m_speed += 2f;

        GameObject.Find("Player").GetComponent<MovementManager>().speed /= 2;
    }
}
