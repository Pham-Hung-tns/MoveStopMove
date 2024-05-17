using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerPatrol : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.ChangeAnim(CacheString.ANIM_RUN);
    }

    public void OnExecute(PlayerController player)
    {
        player.Move();
        player.CheckPatrolToIdle();
    }

    public void OnExit(PlayerController player)
    {
    }
}