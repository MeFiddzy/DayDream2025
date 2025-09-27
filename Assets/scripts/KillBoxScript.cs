using System;
using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        MovementManager movementManager = other.gameObject.GetComponent<MovementManager>();
        if (movementManager != null)
        {
            movementManager.kill();
        }
    }
}
