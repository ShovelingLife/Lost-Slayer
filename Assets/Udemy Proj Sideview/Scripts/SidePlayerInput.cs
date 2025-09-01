using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

using static EPlayerState;

/* 커스텀 키보드 보류) 25-08-07
 
  KeyBoard 클래스에서 KeyControl 클래스를 받아 > KeyBoard.aKey ...
 UI상 현재 매핑이 된 키들을 전부 갖고와서 > Dictionary<string, KeyControl> dict / dict["Move"] > a키, 왼쪽 화살표 등  

*/

public class SidePlayerInput : MonoBehaviour
{
    #region 플레이어 관련
    
    SidePlayer player; 
    Rigidbody2D rb;
    
    PlayerInput input;
    
    #endregion
    
    #region 이동 관련
    
    [SerializeField]
    float moveSpeed = 3.5f;
    
    private Vector2 moveInput;
    
    float xInput;
    
    #endregion

    #region 점프 관련

    // [SerializeField]
    float jumpForce = 5f;

    float yDir;
    
    int jumpCnt = 0;

    #endregion

    #region 슬라이드 관련
    
    [SerializeField] 
    private float slidingSpeed;

    private Vector3 lastSlidePos;

    // [SerializeField] 
    const float waitSlideJumpTime = 0.75f; // 고정

    private float curSlideJumpTime;
    
    bool wasSliding = false;
    
    #endregion

    [SerializeField] private InputActionAsset inputs;


    bool HasReachedMaxJump() => jumpCnt == 2;
    
    void Awake()
    {
        player = GameManager.sidePlayer;
        rb = player.GetComponent<Rigidbody2D>();
        InitInput();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        // Debug.Log($"Cur State : {player.state}");

        if (wasSliding && HasReachedMaxJump())
            curSlideJumpTime += Time.fixedDeltaTime;
        
        if (curSlideJumpTime >= waitSlideJumpTime)
            curSlideJumpTime = jumpCnt = 0;
        // else
        //     jumpCnt = 0;
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        var block = collision.gameObject;
        
        if (block.CompareTag("Wall"))
        {
            // 점프 후 착지
            if (block.transform.rotation.eulerAngles.z == 0f)
            {
                jumpCnt = 0;
                wasSliding = false;
                CheckIfMoving();
            }
            // 슬라이드
            else
            {
                wasSliding = true;
                player.ChangeState(SLIDE);
            }
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (player.state == SLIDE)
            Slide();
    }
    
    public void OnCollisionExit2D(Collision2D other)
    {
    }
    
    void InitInput()
    {
        input = GetComponent<PlayerInput>();
        
        input.actions["Move"].performed += Move;
        input.actions["Move"].canceled += Stop;
        
        // w key > 
        input.actions["Jump"].performed += Jump;
        input.actions["Jump"].canceled += (context =>
        {
            /* 수정 해야할 부분 >

                1. 이동키가 안먹힘
                // Move 슬라이드 방식 생각해봐야함

                2. 슬라이드 로직이 이상함 > 벽 탄 상태서 바닥 착지 시
            
            */
            // 공중에 있다가 벽에 다시 붙어야함
            // Debug.Log($"Player pos : {player.transform.position.y} / Last pos : {(lastSlidePos - new Vector3(0f, 0.01f, 0f)).y}");
            // 타이머 방식 적용할 예정
            
        });
        
        // s key > go down
        
        // shift > dash
        
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
        if (player.state is not (MOVE or IDLE or JUMPFALL or SLIDE))
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
        // if (player.state is SLIDE)
        //     return;
        
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
        switch (player.state)
        {
            case IDLE:
            case MOVE:
                Vector2 velocity = rb.linearVelocity;
                velocity.x = moveInput.x * moveSpeed;
                rb.linearVelocity = velocity;
                break;
            
            case SLIDE:
                Slide();
                break;
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        // 최대 점프 횟수
        if (!HasReachedMaxJump())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            player.animator.SetFloat("velY", rb.linearVelocityY); 
            jumpCnt++;
            
            // 점프 할 시 공격 모션 취소
            if (player.state == ATTACK)
                ChangeState(IDLE);
        }
        #region 점프 테스트 용도
            
        // Debug.Log($"Jump cnt : {jumpCnt}");
            
        #endregion    
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

    // 현재는 콜리전 방식 > 레이캐스트 방식도 가능
    void Slide()
    {
        // Debug.Log($"CurSlideJumpTime: {curSlideJumpTime}");
        lastSlidePos = player.transform.position;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -slidingSpeed, float.MaxValue));
    }
}