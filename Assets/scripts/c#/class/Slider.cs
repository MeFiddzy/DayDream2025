using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        m_procentage = newProcentage;
    }
    
    float getProcentage()
    {
        return m_procentage;
    }
}
