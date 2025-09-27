using System;
using UnityEngine;

public class ShieldItem : Item
{
    private float m_useTimeRemaining;
    private bool m_inUse;
    private GameObject m_shieldObject;
    private Transform m_shieldEmptyTransform;
    private MovementManager m_movementManager;
    
    private static readonly float s_timeOn = 4f;

    private void Awake()
    {
        m_shieldEmptyTransform = transform.Find("ShieldEmpty");
    }

    public UseResult onUse(Vector2 pos, GameObject player)
    {
        m_movementManager = player.GetComponent<MovementManager>();
        
        m_shieldObject = new GameObject();
        m_shieldObject.AddComponent<Transform>();
        m_shieldObject.AddComponent<SpriteRenderer>();
        
        m_shieldObject.transform.localScale = new Vector2(
            0.2393f,
            2.050131f
        );

        m_shieldObject.transform.position = new Vector2(
            m_shieldEmptyTransform.position.x + 0.96f  * (float)m_movementManager.getLastDirection(),
            m_shieldEmptyTransform.position.y + 0.39f * (float)m_movementManager.getLastDirection()
        );
        
        m_inUse = true;
        m_useTimeRemaining = s_timeOn;
        
        return UseResult.SUCCESS;
    }

    private void Update()
    {
        m_useTimeRemaining -= Time.deltaTime;

        if (m_useTimeRemaining <= 0 && m_inUse)
        {
            m_inUse = false;
            return;
        }
        
        
    }
}