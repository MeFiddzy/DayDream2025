using System;
using UnityEngine;

public class KillBoxScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        Movement movement = other.gameObject.GetComponent<Movement>();
        if (movement != null)
        {
            movement.Kill();
        }
    }
}
