using System;
using UnityEngine;

class ShieldItem : Item
{
    private float m_useTimeRemaining;
    private bool m_inUse;
    private GameObject m_shieldObject;
    private Transform m_shieldEmptyTransform;
    private MovementManager m_movementManager;
    
    private static readonly float s_timeOn = 4f;

    public override string getName()
    {
        return "Shield";
    }
    
    public ShieldItem(){ }

    private void onStart()
    {
        m_shieldEmptyTransform = GameObject.Find("ShieldEmpty").GetComponent<Transform>();
    }

    public override Item getCopy()
    {
        return new ShieldItem();
    }

    public UseResult onUse(Vector2 pos)
    {
        m_movementManager = GameObject.Find("Player").GetComponent<MovementManager>();
        
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

    private void onFrame()
    {
        if (!m_inUse)
            return;
        
        m_useTimeRemaining -= Time.deltaTime;

        if (m_useTimeRemaining <= 0)
        {
            UnityEngine.Object.Destroy(m_shieldObject);
            m_inUse = false;
            return;
        }
        
        m_shieldObject.transform.position = new Vector2(
            m_shieldEmptyTransform.position.x + 0.96f  * (float)m_movementManager.getLastDirection(),
            m_shieldEmptyTransform.position.y + 0.39f * (float)m_movementManager.getLastDirection()
        );
        
        if (m_movementManager.getLastDirection() == MovementManager.Direction.LEFT)
            m_shieldObject.GetComponent<SpriteRenderer>().flipX = true;
    }
}