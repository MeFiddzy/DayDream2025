using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CgheckCastel : MonoBehaviour
{
    public void Update()
    {
        if (transform.position.y < -10)
        {
            SceneManager.LoadScene("Level5",  LoadSceneMode.Single);
        }
    }
}