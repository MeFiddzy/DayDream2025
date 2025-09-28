using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string name;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<MovementManager>() == null)
            return;
        
        print(gameObject.name);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
