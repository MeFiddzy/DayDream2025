using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    private static readonly float s_maxHP = 30f;

    // enum intern pentru direcție
    public enum Direction
    {
        LEFT = -1,
        RIGHT = 1
    }

    [SerializeField] private Direction m_goingDirection = Direction.LEFT;
    [SerializeField] public float m_speed = 1f;               // viteza slime-ului
    [SerializeField] private float m_edgeCheckDistance = 0.5f; // distanța pentru raycast
    [SerializeField] private LayerMask groundMask;             // layer-ul solului

    private float m_remainingHP = s_maxHP;
    private Rigidbody2D m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

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
        // Punctul din față și puțin sub slime
        Vector2 checkPos = new Vector2(
            transform.position.x + 0.3f * (int)m_goingDirection,
            transform.position.y - 0.5f
        );

        // Debug: vezi raza în Scene View
        Debug.DrawRay(checkPos, Vector2.down * m_edgeCheckDistance, Color.red);

        // Verificăm dacă e sol sub slime
        bool hasGround = Physics2D.Raycast(checkPos, Vector2.down, m_edgeCheckDistance, groundMask);

        // Daca nu e sol → schimbă direcția
        if (!hasGround)
        {
            m_goingDirection = m_goingDirection == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }

        // Mișcare lentă și stabilă folosind Rigidbody2D
        m_rigidbody.velocity = new Vector2(m_speed * (int)m_goingDirection, m_rigidbody.velocity.y);
    }
}