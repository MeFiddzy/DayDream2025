using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static HashSet<KeyCode> attackKeyCodes = new HashSet<KeyCode>();

    public float m_remainingAttackCooldown = 0f;

    private SpriteRenderer m_playerSpriteRenderer;

    private readonly Dictionary<MovementManager.Direction, GameObject> m_dirAttHBox =
        new Dictionary<MovementManager.Direction, GameObject>
        {
            { MovementManager.Direction.LEFT, GameObject.Find("LAttHBox") },
            { MovementManager.Direction.RIGHT, GameObject.Find("RAttHBox") },
        };
    
    private void Awake()
    {
        m_playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        m_remainingAttackCooldown -= Time.deltaTime;

        if (m_remainingAttackCooldown > 0)
            return;

        if (MovementManager.checkAllKeys(attackKeyCodes, MovementManager.StateType.PRESSING))
        {
            
        }
    }
}