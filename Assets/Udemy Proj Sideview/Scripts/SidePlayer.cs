using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using static EPlayerState;

public class SidePlayer : MonoBehaviour
{
    #region Moving

    // Transform trans;

    // UI 매니저에 옮길 예정
    Dictionary<KeyCode, Action> downKeys = new Dictionary<KeyCode, Action>();
    Dictionary<KeyCode, Action> upKeys = new Dictionary<KeyCode, Action>();


    public EPlayerState state
    {
        get;
        set;
    }

    #endregion

    #region Animation 

    public Animator animator;

    #endregion

    void Awake()
    {

        // trans = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();

        // // Custom Key
        // // curKeys.Add("Move", new List<KeyCode>(new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }));
        // upKeys.Add(KeyCode.LeftArrow, Move); upKeys.Add(KeyCode.RightArrow, Move);
        // upKeys.Add(KeyCode.Space, Jump);

        // downKeys.Add(KeyCode.LeftArrow, Stop); downKeys.Add(KeyCode.RightArrow, Stop);
    }

    void Update()
    {
        animator.SetInteger("state", (int)state);
    }

    // void Update()
    // {
    //     foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
    //     {
    //         if (key == KeyCode.None)
    //             continue;

    //         var name = key.ToString();

    //         if ((Input.GetButton(name) || Input.GetButtonDown(name))
    //             && downKeys.ContainsKey(key))
    //             downKeys[key].Invoke();

    //         if (Input.GetButtonUp(name))
    //             upKeys[key].Invoke();
    //     }

    //     /* 살짝 어색함 > 오른쪽으로 이동 후 잠시 멈춘 후 방향 전환 시 idle 애니메이션 실행
    //                     수정 완료 25.07.25
    //                 */
    // }
}
