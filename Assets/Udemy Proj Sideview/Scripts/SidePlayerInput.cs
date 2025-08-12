using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static EPlayerState;

/* 커스텀 키보드 보류) 25-08-07
 
  KeyBoard 클래스에서 KeyControl 클래스를 받아 > KeyBoard.aKey ...
 UI상 현재 매핑이 된 키들을 전부 갖고와서 > Dictionary<string, KeyControl> dict / dict["Move"] > a키, 왼쪽 화살표 등  

*/

public class SidePlayerInput : MonoBehaviour
{
    SidePlayer player; 
    Rigidbody2D rb;
    
    PlayerInput input;
    
    [SerializeField]
    float moveSpeed = 3.5f;

    // [SerializeField]
    float jumpForce = 5f;

    float xInput;
    float yDir;
    
    int jumpCnt = 0;
    private Vector2 moveInput;

    [SerializeField] private InputActionAsset inputs;

    
    void Awake()
    {
        player = GameManager.sidePlayer;
        rb = player.GetComponent<Rigidbody2D>();
        InitInput();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            jumpCnt = 0;
            CheckIfMoving();
        }
    }

    void InitInput()
    {
        input = GetComponent<PlayerInput>();
        input.actions["Move"].performed += Move;
        input.actions["Move"].canceled += Stop;
        
        // w key > 
        input.actions["Jump"].performed += Jump;
        
        // s key > go down
        
        // space > line
        
        
        // mouse click
        input.actions["Attack"].performed += Attack;
        
        
        // foreach (var events in input.actionEvents)
        // {
        //     Debug.Log($"Name : {events.actionId}");
        // }
    }

    void ChangeState(EPlayerState newState) => player.ChangeState(newState);
        
    void Move(InputAction.CallbackContext context) => Move(context.ReadValue<Vector2>());

    void Move(Vector2 dir)
    {
        if (player.state == ATTACK)
            return;
        
        moveInput = dir;
        player.transform.rotation = new Quaternion(0f, (moveInput.x > 0f) ? 0f : 180f, 0f, 0f);
        ChangeState(MOVE);
        
        /* 아래와 같이 할 수도 있지만 (미러링 방식) 나중에 텍스처 등 그래픽적으로 깨질 수가 있음
         Debug.Log(moveInput.x);
         player.transform.localScale = new Vector3(moveInput.x, 1f, 1f); */
    }

    void CheckIfMoving()
    {
        var keyBoard = Keyboard.current;
        
        // 커스터마이징 할 시 변경 해야함
        if (keyBoard.aKey.isPressed)
            Move(new Vector2(-1f, 0f));
        
        else if (keyBoard.dKey.isPressed)
            Move(new Vector2(1f, 0f));
        
        else
            player.ChangeState(IDLE);
    }

    void Stop(InputAction.CallbackContext context)
    {
        moveInput.x = 0f;
        
        if (jumpCnt == 0)
            ChangeState(IDLE);
    }

    void HandleMovement()
    {
        // Debug.Log(player.state.ToString());
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput.x * moveSpeed;
        rb.linearVelocity = velocity;
    }

    void Jump(InputAction.CallbackContext context)
    {
        // 최대 점프 횟수
        if (jumpCnt < 2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            player.animator.SetFloat("velY", rb.linearVelocityY); 
            jumpCnt++;
            
            // 점프 할 시 공격 모션 취소
            if (player.state == ATTACK)
                ChangeState(IDLE);
        }
    }

    void GoDown()
    {
        
    }
    
    // shift or 이동키 더블 푸쉬
    void Dash()
    {
        
    }

    void Attack(InputAction.CallbackContext context)
    {
        // Debug.Log($"Velocity : {rb.linearVelocity}");
        moveInput = Vector2.zero;
        ChangeState(ATTACK);

        // 클릭 위치에 따라 캐릭터 주시하는 방향 전환

    }

    public void AttackEnd()
    {
        ChangeState(IDLE);
        
        // Debug.Log("attack end");
        CheckIfMoving();

        /* 아래는 안먹히는 설정
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D))
            Debug.Log("moving"); */
    }
}