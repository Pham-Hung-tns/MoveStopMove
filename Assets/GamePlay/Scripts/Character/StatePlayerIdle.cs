using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerIdle : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.ChangeAnim(CacheString.ANIM_IDLE);
    }

    public void OnExecute(PlayerController player)
    {
        //TODO: patrol thi khong attack nua - sua cho nay
        player.CheckIdleToPatrol();
        player.CheckIdletoAttack();
    }

    public void OnExit(PlayerController player)
    {
    }
}