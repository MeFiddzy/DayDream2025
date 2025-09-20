using System;
using System.Collections.Generic;
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
    public uint wallJumps = 2;
    public float ratioDashToAfterTime = 0.3f;
    
    public HashSet<KeyCode> dashKeys = new HashSet<KeyCode>{KeyCode.X, KeyCode.Q};
    public HashSet<KeyCode> jumpKeys = new HashSet<KeyCode>{KeyCode.UpArrow, KeyCode.W, KeyCode.Space};
    public HashSet<KeyCode> rightKeys = new HashSet<KeyCode>{KeyCode.RightArrow, KeyCode.D};
    public HashSet<KeyCode> leftKeys = new HashSet<KeyCode>{KeyCode.LeftArrow, KeyCode.A};

    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_spriteRenderer;
    private Transform m_cameraTransform;
    private Transform m_respawn;
    
    private Direction m_lastDirection = Direction.LEFT;
    private Direction m_lastWallJumpDirection;
    

    private uint m_wallJumpsUsed = 0;
    private bool m_dashedNow = false;
    private bool m_airJumpedThisAirtime = false;
    private float m_dashCooldownTimeLeft = 0.0f;
    private float m_dashTimeRemaining = 0.0f;
    private float m_wallJumpAfterTime = 0.0f;
    
    public void Kill()
    {
        transform.position = new Vector2(m_respawn.position.x, m_respawn.position.y);
        
        m_dashedNow = false;
        m_dashTimeRemaining = 0.0f;
        m_dashCooldownTimeLeft = 0.0f;

        m_wallJumpsUsed = 0;
        m_wallJumpAfterTime = 0.0f;
        
        m_airJumpedThisAirtime = false;
    }
    
    private void Awake()
    {
        m_respawn = GameObject.Find("Respawn").GetComponent<Transform>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_cameraTransform = Camera.main.transform;
    }

    private enum StateType
    {
        DOWN,
        PRESSING
    }

    private bool checkAllKeys(HashSet<KeyCode> keys, StateType state)
    {
        foreach (KeyCode key in keys)
        {
            if (state == StateType.PRESSING)
            {
                if (Input.GetKey(key))
                {
                    return true;
                }
            }
            else if (state == StateType.DOWN)
            {
                if (Input.GetKeyDown(key))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Kill();
        }
        
        m_cameraTransform.position = new Vector2(
            transform.position.x,
            m_cameraTransform.position.y
        );
        
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

        Debug.DrawRay(
            new Vector2(transform.position.x + transform.localScale.x / 2, transform.position.y),
            Vector2.right,
            Color.red
        );
        
        Debug.DrawRay(
            new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y),
            Vector2.left,
            Color.red
        );
        
        Func<float, RaycastHit2D> raycast4OnGround = errorX => {
            Debug.DrawRay(
                new Vector2(transform.position.x + errorX, transform.position.y - transform.localScale.y / 2),
                Vector2.down,
                Color.red
            );
            
            return Physics2D.Raycast(
                new Vector2(transform.position.x + errorX, transform.position.y - transform.localScale.y / 2),
                Vector2.down,
                0.2373794f,
                groundLayer
            );
        };
        
        bool onGround = raycast4OnGround(transform.localScale.x / 2 - 0.15f) || raycast4OnGround(transform.localScale.x / -2  + 0.15f);
        bool canDash = m_dashCooldownTimeLeft <= 0f;

        if (onGround)
        {
            m_dashedNow = false;
            m_wallJumpsUsed = 0;
            m_airJumpedThisAirtime = false;
        }

        if (checkAllKeys(jumpKeys, StateType.DOWN))
        {
            if (onGround)
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 2);
            else if (Physics2D.Raycast(
                         new Vector2(transform.position.x + transform.localScale.x / 2, transform.position.y),
                         Vector2.right,
                         0.2373794f,
                         groundLayer
                     ) && m_wallJumpsUsed < wallJumps)
            {
                m_lastWallJumpDirection = Direction.LEFT;
                m_wallJumpAfterTime = dashDuration;
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 1.2f);
                m_wallJumpsUsed++;
                return;
            }
            else if (Physics2D.Raycast(
                         new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y),
                         Vector2.left,
                         0.2373794f,
                         groundLayer
                     )&& m_wallJumpsUsed < wallJumps)
            {
                m_lastWallJumpDirection = Direction.RIGHT;   
                m_wallJumpAfterTime = dashDuration;
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 1.2f);
                m_wallJumpsUsed++;
                return;
            }
            else if (!m_airJumpedThisAirtime)
            {
                m_airJumpedThisAirtime = true;
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 1.5f);

                if (checkAllKeys(dashKeys, StateType.PRESSING) && m_dashedNow)
                {
                    canDash = false;

                    m_dashedNow = false;
                    m_dashTimeRemaining = dashDuration;
                    m_dashCooldownTimeLeft = dashCooldown;

                }
            }
        }
            

        Action<float> dashLogic = x => {
            m_rigidbody.velocity = new Vector2(x, m_rigidbody.velocity.y);
        };

        if (m_wallJumpAfterTime > 0.0f)
        {
            dashLogic(dashPower * 20 * ratioDashToAfterTime * (float)m_lastWallJumpDirection);
            m_wallJumpAfterTime -= Time.deltaTime;
            
            return;
        }
        
        if (m_dashTimeRemaining > 0f)
        {
            dashLogic(dashPower * 20 * (float)m_lastDirection);
            return;
        }

        if (checkAllKeys(dashKeys, StateType.PRESSING) && canDash)
        {
            m_dashedNow = true;
            
            m_dashTimeRemaining = dashDuration;
            m_dashCooldownTimeLeft = dashCooldown;
        }

        float horizontalInput = 0f;
        if (checkAllKeys(leftKeys, StateType.PRESSING))
        {
            m_lastDirection = Direction.LEFT;
            horizontalInput = onGround ? -1f : -0.7f;
        }
        else if (checkAllKeys(rightKeys, StateType.PRESSING))
        {
            m_lastDirection = Direction.RIGHT;
            horizontalInput = onGround ? 1f : 0.7f;
        }
        
        m_rigidbody.velocity = new Vector2(horizontalInput * speed * 15f, m_rigidbody.velocity.y);
        
    }
}
