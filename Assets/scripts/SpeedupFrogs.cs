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

        foreach (var obj in GameObject.FindGameObjectsWithTag("Broasca"))
        {
            obj.GetComponent<SlimeEnemy>().m_speed += 2f;
        }
        
        GameObject.Find("Player").GetComponent<MovementManager>().speed /= 2;
    }
}
