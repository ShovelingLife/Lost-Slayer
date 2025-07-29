using UnityEngine;
using UnityEngine.InputSystem;

using static EPlayerState;

public class SidePlayerInput : MonoBehaviour
{
    SidePlayer player; 
    Rigidbody2D rb;
    
    PlayerInput input;
    
    [SerializeField]
    float moveSpeed = 3.5f;

    [SerializeField]
    float jumpForce = 8f;

    float xInput;
    float yDir;
    
    int jumpCnt = 0;

    void Awake()
    {
        player = GameManager.sidePlayer;
        rb = player.GetComponent<Rigidbody2D>();
        InitInput();
    }

    void Update()
    {
    }

    void InitInput()
    {
        input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += Move;
        // input.actions["Move"].canceled += Stop;
        
        input.actions["Jump"].performed += Jump;
        
        // foreach (var events in input.actionEvents)
        // {
        //     Debug.Log($"Name : {events.actionId}");
        // }
    }
        
    void Move(InputAction.CallbackContext context)
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
        // animator?.SetInteger("state", (int)MOVE);
        // Debug.Log($" Cur speed : {xInput}");
        // transform.Rotate(new Vector3(0f, 180f, 0f));
        player.state = MOVE;
        transform.rotation = new Quaternion(0f, (xInput > 0f) ? 0f : 180f, 0f, 0f);
    }

    void Stop(InputAction.CallbackContext context)
    {
        if (xInput != 0f)
            return;
        
        player.state = IDLE;
    }

    void Jump(InputAction.CallbackContext context)
    {
        // 최대 점프 횟수
        if (jumpCnt < 2)
        {
            player.animator.SetFloat("velY", rb.linearVelocityY); 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCnt++;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            jumpCnt = 0;
            player.state = IDLE;
        }
    }
}
