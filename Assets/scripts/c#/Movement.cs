using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 0.3f;
    public float jumpPower = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        bool onGround = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2), 
            Vector2.down, 
            0.1f, 
            groundLayer
        );

        if (Input.GetKeyDown(KeyCode.UpArrow) && onGround)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, jumpPower * 2 + m_rigidbody.velocity.y);
        }
        
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) 
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) 
            horizontalInput = 1f;
        else
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
            return;
        }


        m_rigidbody.velocity = new Vector2(horizontalInput * speed * 15, m_rigidbody.velocity.y);
    }
}