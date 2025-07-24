using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnityEngine;
using static EPlayerState;

public class SidePlayer : MonoBehaviour
{
    #region Moving

    // Transform trans;
    
    // UI 매니저에 옮길 예정
    Dictionary<string, List<KeyCode>> curKeys = new Dictionary<string, List<KeyCode>>();
    
    Rigidbody2D rb;

    [SerializeField]
    float moveSpeed = 3.5f;

    [SerializeField]
    float jumpForce = 8f;

    float xInput;
    float yDir;
    // bool isFaced;
    
    [SerializeField]
    int jumpCnt = 0;

    #endregion

    #region Animation 

    Animator animator;

    public EPlayerState state
    {
        get;
        private set;
    }

    #endregion

    void Awake()
    {
        // trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        // Custom Key
        curKeys.Add("Move", new List<KeyCode>(new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }));
    }

    void Update()
    {
        CheckMove();
        CheckJump();
    }

    void CheckMove()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);

        #region Animation

        // yDir = (velX > 0f) ? 0f : 180f;

        foreach (var key in curKeys["Move"])
        {
            if (Input.GetKeyDown(key) || Input.GetKey(key))
            {
                state = MOVE;
                // Debug.Log($" Cur speed : {xInput}");
                // transform.Rotate(new Vector3(0f, 180f, 0f));
                transform.rotation = new Quaternion(0f, (xInput > 0f) ? 0f : 180f, 0f, 0f);
                
                // 살짝 어색함 > 오른쪽으로 이동 후 잠시 멈춘 후 방향 전환 시 idle 애니메이션 실행

            }
            else if (Input.GetKeyUp(key) && xInput == 0f)
                state = IDLE;
        }
        // if (!isFaced && xInput < 0f)
        //     transform.Rotate(new Vector3(0f, 180f, 0f));

        // else
        //     transform.Rotate(Vector3.zero);
            
        animator?.SetInteger("state", (int)state);
        #endregion
    }

    void CheckJump()
    {
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
