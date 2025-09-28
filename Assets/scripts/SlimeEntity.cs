using System;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    private static readonly float s_maxHP = 30f;

    public MovementManager.Direction m_goingDirection = MovementManager.Direction.LEFT;

    [SerializeField] public float m_speed = 0.5f;
    [SerializeField] public float m_edgeCheckDistance = 2.4f;

    private float m_remainingHP = s_maxHP;

    public void DealDamage(float damage)
    {
        m_remainingHP -= damage;

        if (m_remainingHP <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 checkPos = new Vector2(
            transform.position.x + 0.2f * (float)m_goingDirection,
            transform.position.y
        );

        bool hasGround = Physics2D.Raycast(checkPos, Vector2.down, m_edgeCheckDistance);

        if (!hasGround)
        {
            m_goingDirection = m_goingDirection == MovementManager.Direction.LEFT 
                ? MovementManager.Direction.RIGHT 
                : MovementManager.Direction.LEFT;
        }

        transform.position = new Vector2(transform.position.x + (float)m_goingDirection * m_speed, transform.position.y);
    }
}