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
    public float coyoteTime = 0.1f;
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
    public HashSet<KeyCode> reloadINIKeys = new HashSet<KeyCode>{KeyCode.Delete};
    public HashSet<KeyCode> resetKeys = new HashSet<KeyCode>{KeyCode.R};

    private Rigidbody2D m_rigidbody;
    private Transform m_dashBGTransform;
    private Transform m_cameraTransform;
    private Transform m_respawnTransform;
    private Transform m_followPlayer;
    private Transform m_voidKillTransform;
    
    private Direction m_lastDirection = Direction.LEFT;
    private Direction m_lastWallJumpDirection;
    

    private uint m_wallJumpsUsed = 0;
    private bool m_dashedNow = false;
    private bool m_airJumpedThisAirtime = false;
    private float m_dashCooldownTimeLeft = 0.0f;
    private float m_dashTimeRemaining = 0.0f;
    private float m_wallJumpAfterTime = 0.0f;
    private float m_coyoteTime = 0.0f;
    
    public void kill()
    {
        transform.position = new Vector2(m_respawnTransform.position.x, m_respawnTransform.position.y);
        
        m_dashedNow = false;
        m_dashTimeRemaining = 0.0f;
        m_dashCooldownTimeLeft = 0.0f;

        m_wallJumpsUsed = 0;
        m_wallJumpAfterTime = 0.0f;
        
        m_airJumpedThisAirtime = false;
    }
    
    public void Awake()
    {
        m_voidKillTransform = GameObject.Find("KillVoid").GetComponent<Transform>();
        m_dashBGTransform = GameObject.Find("DashSlider").GetComponent<Transform>();
        m_followPlayer = GameObject.Find("FollowPlayer").GetComponent<Transform>();
        m_respawnTransform = GameObject.Find("Respawn").GetComponent<Transform>();
        m_cameraTransform = Camera.main.transform;
        
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        reloadConfig();
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

    private HashSet<KeyCode> INIArrayToKeys(string[] arr)
    {
        HashSet<KeyCode> keys = new HashSet<KeyCode>();

        foreach (string key in arr)
        {
            if (Enum.TryParse(key, true, out KeyCode keyCode))
            {
                keys.Add(keyCode);
            }
        }

        return keys;
    }

    
    public void Update()
    {
        // coyote time decrease
        m_coyoteTime = Math.Max(m_coyoteTime - Time.deltaTime, 0);
        
        if (checkAllKeys(reloadINIKeys, StateType.DOWN))
        {
            reloadConfig();
        }
        
        // Dash slider follow
        m_followPlayer.position = transform.position;
        
        if (checkAllKeys(resetKeys, StateType.DOWN))
        {
            kill();
        }
        
        // Camera follow
        m_cameraTransform.position = new Vector2(
            transform.position.x,
            m_cameraTransform.position.y
        );
        
        // Dash time logic
        m_dashCooldownTimeLeft = Math.Max(m_dashCooldownTimeLeft - Time.deltaTime, 0f);
        m_dashTimeRemaining = Math.Max(m_dashTimeRemaining - Time.deltaTime, 0f);

        // Dash slider logic
        m_dashBGTransform.localScale = new Vector2(
            0.8359f * ((dashCooldown - Math.Min(m_dashCooldownTimeLeft, dashCooldown)) / dashCooldown),
            m_dashBGTransform.localScale.y
        );

        // Debug Rays
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
        
        // Jumping logic
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

        // boolean init
        bool onGround = raycast4OnGround(transform.localScale.x / 2 - 0.15f) ||
                        raycast4OnGround(transform.localScale.x / -2 + 0.15f);
        
        bool canJump = onGround || m_coyoteTime > 0f;
        
        bool canDash = m_dashCooldownTimeLeft <= 0f;
        
        if (onGround)
        {
            m_coyoteTime = coyoteTime;
            m_dashedNow = false;
            m_wallJumpsUsed = 0;
            m_airJumpedThisAirtime = false;
        }

        if (checkAllKeys(jumpKeys, StateType.DOWN))
        {
            if (canJump)
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 2);
            // Walljumping
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
            // airjumping
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
            

        // Dashing
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

        if (checkAllKeys(dashKeys, StateType.DOWN) && canDash)
        {
            m_dashedNow = true;
            
            m_dashTimeRemaining = dashDuration;
            m_dashCooldownTimeLeft = dashCooldown;
        }

        // left & right
        float horizontalInput = 0f;
        if (checkAllKeys(leftKeys, StateType.PRESSING))
        {
            m_lastDirection = Direction.LEFT;
            horizontalInput = canJump ? -1f : -0.9f;
        }
        else if (checkAllKeys(rightKeys, StateType.PRESSING))
        {
            m_lastDirection = Direction.RIGHT;
            horizontalInput = canJump ? 1f : 0.9f;
        }
        
        m_rigidbody.velocity = new Vector2(horizontalInput * speed * 15f, m_rigidbody.velocity.y);
        m_voidKillTransform.position = new Vector2(
            transform.position.x,
            m_voidKillTransform.position.y
        );
    }
    
    private void reloadConfig()
    {
        INIFile configFile = INIFile.loadFile(Application.dataPath + "/config/player_stats.ini");
            
        speed = (float)Convert.ToDouble(configFile["_"]["speed"]);
        coyoteTime = (float)Convert.ToDouble(configFile["_"]["coyote_time"]);
            
        jumpPower = (float)Convert.ToDouble(configFile["jump"]["jump_power"]);
        wallJumps = Convert.ToUInt32(configFile["jump"]["wall_jumps"]);
        ratioDashToAfterTime = (float)Convert.ToDouble(configFile["jump"]["ratio_dash_to_wall_jump_after"]);
            
        dashPower = (float)Convert.ToDouble(configFile["dash"]["dash_power"]);
        dashCooldown = (float)Convert.ToDouble(configFile["dash"]["dash_cooldown"]);
        dashDuration = (float)Convert.ToDouble(configFile["dash"]["dash_duration"]);
            
        print("Reset Stats");
            
        INIFile keystrokes = INIFile.loadFile(Application.dataPath + "/config/keystrokes.ini");
            
        dashKeys = INIArrayToKeys(keystrokes.loadArray("dash_keys"));
        jumpKeys = INIArrayToKeys(keystrokes.loadArray("jump_keys"));
        leftKeys = INIArrayToKeys(keystrokes.loadArray("left_keys"));
        rightKeys = INIArrayToKeys(keystrokes.loadArray("right_keys"));
        reloadINIKeys = INIArrayToKeys(keystrokes.loadArray("reload_ini_keys"));
        resetKeys = INIArrayToKeys(keystrokes.loadArray("reset_keys"));
    }
}
