using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryOpenManager : MonoBehaviour
{
    public HashSet<KeyCode> invOpenKeys = new HashSet<KeyCode>();
    public HashSet<KeyCode> dropKeys = new HashSet<KeyCode>();
    public HashSet<KeyCode> useKeys = new HashSet<KeyCode>();

    private InventoryManager m_invManager;
    private uint m_curStep = 0;
    private float m_waitTime = 0f;
    private bool m_shouldWait = false;
    
    private TextMeshProUGUI  m_text;
    
    
    private static readonly uint s_maxSteps = 5;
    private static readonly uint s_maxWait = 5;
    
    public void Awake()
    {
        m_text = GameObject.Find("ItemText").GetComponent<TextMeshProUGUI >(); 
        m_invManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        
        m_invManager.addItem(UItems.getItemById(0));
        m_invManager.addItem(UItems.getItemById(0));
        m_invManager.addItem(UItems.getItemById(0));
        m_invManager.addItem(UItems.getItemById(0));
    }

    public void Update()
    {
        m_waitTime -= Time.deltaTime;
        
        List<KeyCode> numberKeys = new List<KeyCode>
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
            KeyCode.Alpha9, KeyCode.Alpha0
        };

        for (int i = 0; i < 1; i++)
        {
            if (Input.GetKeyDown(numberKeys[i]))
            {
                string itemName = m_invManager.getItem(i).getName();
                m_text.text = itemName; 
                m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, 0f); 
                print(m_text.text);
            }
        }
        m_curStep++;

        m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, Math.Min(1f * s_maxSteps / m_curStep, 1.0f));
        
        if (m_curStep >= s_maxSteps) 
        { 
             m_waitTime = s_maxWait;
             m_shouldWait = true;
        }

        if (m_waitTime <= 0 && m_shouldWait)
        {
            m_shouldWait = false;
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, 0f);
        }
    }
}