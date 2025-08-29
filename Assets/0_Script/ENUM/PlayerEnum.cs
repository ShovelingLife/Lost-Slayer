using UnityEngine;

// 보완 필요함 > enum 순서가 뒤바뀔수 있음
public enum EPlayerState
{
    DEATH = -1,
    IDLE = 0,
    MOVE,
    JUMP,
    ATTACK,
    JUMPFALL,
    SLIDE
}

public enum ETest
{
    a = 1,
    b,
    c,
    d,
    e,
}