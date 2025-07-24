using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnityEngine;

public class SidePlayer : MonoBehaviour
{
    #region Moving
    Rigidbody2D rb;

    [SerializeField]
    float moveSpeed = 3.5f;

    [SerializeField]
    float jumpForce = 8f;

    float xInput;
    
    [SerializeField]
    int jumpCnt = 0;

    #endregion

    #region Animation

    Animator anim;

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (jumpCnt < 2)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCnt++;               
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            jumpCnt = 0;
        }
    }
}
