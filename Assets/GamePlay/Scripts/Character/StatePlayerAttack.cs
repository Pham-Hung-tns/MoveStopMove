using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.ChangeAnim(CacheString.ANIM_ATTACK);
    }

    public void OnExecute(PlayerController player)
    {
        player.Attack();
        player.CheckIdleToPatrol();
    }

    public void OnExit(PlayerController player)
    {
    }
}