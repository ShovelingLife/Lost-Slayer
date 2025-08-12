using System;
using UnityEngine;

public class SidePlayerAnim : MonoBehaviour
{
    private SidePlayer player;
    
    private void Awake()
    {
        player = GetComponentInParent<SidePlayer>();
    }

    void AttackEnd() => player.sidePlayerInput.AttackEnd();
}
