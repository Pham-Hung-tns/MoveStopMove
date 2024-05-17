using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyIdle : IState
{
    public void OnEnter(EnemyController enemy)
    {
        enemy.ChangeAnim(CacheString.ANIM_IDLE);
        enemy.EnemyStopMoving();
        enemy.RestartTimeCounting();
    }

    public void OnExecute(EnemyController enemy)
    {
        enemy.CheckIdletoAttack();
        enemy.CheckIdletoPatrol();
    }

    public void OnExit(EnemyController enemy)
    {
    }
}