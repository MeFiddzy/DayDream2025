using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName = "ExempleLevelScene";
    
    private Button m_button;
    
    public void Awake()
    {
        m_button = GetComponent<Button>();
        
        m_button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
