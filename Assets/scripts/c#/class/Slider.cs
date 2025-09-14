using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("Unfinished")] 
public class Slider
{
    private GameObject m_background;
    private GameObject m_frontground;
    private float m_procentage = 0.0f;

    void setBG(GameObject bg)
    {
        m_background = bg;
    }

    void setFG(GameObject fg)
    {
        m_frontground = fg;
    }
    
    GameObject getBG()
    {
        return m_background;
    }

    GameObject getFG()
    {
        return m_frontground;
    }
    
    public static Slider load(string key)
    {
        Slider slider = new Slider();
            
        slider.setBG(GameObject.Find("Background" + key));
        slider.setFG(GameObject.Find("Frontground" + key));
            
        return slider;
    }

    void setProcentage(float newProcentage)
    {
        if (m_procentage == newProcentage)
            return;
        
        m_frontground.transform.localScale = new Vector2(
            m_frontground.transform.localScale.x, 
            m_frontground.transform.localScale.y * (newProcentage / m_procentage)
        );

        m_frontground.transform.position = new Vector2(
            m_frontground.transform.position.x,
            m_frontground.transform.position.y * (newProcentage / m_procentage) / 2
        );
        
        m_procentage = newProcentage;
    }
    
    float getProcentage()
    {
        return m_procentage;
    }
}
