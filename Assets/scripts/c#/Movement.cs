using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private enum Direction
    {
        LEFT = -1,
        RIGHT = 1
    }

    public float speed = 0.3f;
    public float dashPower = 1f;
    public float jumpPower = 5f;
    public LayerMask groundLayer;
    public float dashCooldown = 4.5f;
    public float dashDuration = 0.2f;
    public string dashKey = "PlayerDash";

    private Rigidbody2D m_rigidbody;

    private Slider m_dashSlider = new Slider();
    
    private Direction m_lastDirection = Direction.LEFT;
    private float m_dashCooldownTimeLeft = 0.0f;
    private float m_dashTimeRemaining = 0.0f;
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_dashSlider = Slider.load(dashKey);
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_dashCooldownTimeLeft = Math.Max(m_dashCooldownTimeLeft - Time.deltaTime, 0f);
        m_dashTimeRemaining = Math.Max(m_dashTimeRemaining - Time.deltaTime, 0f);

        if (m_dashCooldownTimeLeft <= 0)
        {
            m_spriteRenderer.color = Color.green;
        }
        else
        {
            m_spriteRenderer.color = Color.white;
        }

        Func<float, RaycastHit2D> raycast4OnGround = (float errorX) => {
            return Physics2D.Raycast(
                new Vector2(transform.position.x + errorX, transform.position.y - transform.localScale.y / 2),
                Vector2.down,
                0.1f,
                groundLayer
            );
        };
        
        bool onGround = raycast4OnGround(0) || raycast4OnGround(transform.localScale.x / 2) || raycast4OnGround(transform.localScale.x / -2);
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 2 + m_rigidbody.velocity.y);
        }
        
        if (m_dashTimeRemaining > 0f)
        {
            m_rigidbody.velocity = new Vector2(dashPower * 20 * (float)m_lastDirection, m_rigidbody.velocity.y);
            return;
        }

        if (Input.GetKeyDown(KeyCode.X) && m_dashCooldownTimeLeft <= 0f)
        {
            m_dashTimeRemaining = dashDuration;
            m_dashCooldownTimeLeft = dashCooldown;
        }

        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_lastDirection = Direction.LEFT;
            horizontalInput = onGround ? -1f : -0.5f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_lastDirection = Direction.RIGHT;
            horizontalInput = onGround ? 1f : 0.5f;
        }

        m_rigidbody.velocity = new Vector2(horizontalInput * speed * 15f, m_rigidbody.velocity.y);
    }
}
