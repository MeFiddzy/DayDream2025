using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public MovementManager.Direction curDirection;

    private Transform m_swordTransform;
    private SpriteRenderer m_swordRenderer;
    
    private MovementManager.Direction m_lastRecordedDirection = MovementManager.Direction.LEFT;
    private MovementManager m_playerMovementManager;
    
    private float m_playerXSize;
    
    public void Awake()
    {
        m_playerXSize = GameObject.Find("Player").GetComponent<Transform>().localScale.x + 0.6373794f;
        m_playerMovementManager = GameObject.Find("Player").GetComponent<MovementManager>();
        m_swordTransform = GameObject.Find("Sword").GetComponent<Transform>();
        m_swordRenderer = GameObject.Find("Sword").GetComponent<SpriteRenderer>();
    }
    
    public void Update()
    {
        if (m_lastRecordedDirection != curDirection)
        {
            if (curDirection == MovementManager.Direction.LEFT)
            {
                m_swordRenderer.flipX = false;
                m_swordTransform.position = new Vector2(m_swordTransform.position.x - m_playerXSize, m_swordTransform.position.y);
            }
            else
            {
                m_swordRenderer.flipX = true;
                m_swordTransform.position = new Vector2(m_swordTransform.position.x + m_playerXSize, m_swordTransform.position.y);
            }
        }
        
        m_lastRecordedDirection = curDirection;

        if (MovementManager.checkAllKeys(m_playerMovementManager.useSwordKeys, MovementManager.StateType.DOWN))
        {
            
        }
    }
}
