using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyAttack : IState
{
    public void OnEnter(EnemyController enemy)
    {
        enemy.ChangeAnim(CacheString.ANIM_ATTACK);
        enemy.Attack();
        enemy.EnemyStopMoving();
        enemy.RestartTimeCounting();
    }

    public void OnExecute(EnemyController enemy)
    {
        enemy.CheckIfAttackIsDone();
    }

    public void OnExit(EnemyController enemy)
    {
    }
}