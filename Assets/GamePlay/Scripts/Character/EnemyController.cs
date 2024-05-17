using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : Character, IHit
{
    #region Parameter

    [SerializeField] private Animator enemyAnimation;
    [SerializeField] private LayerMask whatIsPlayer;
    public NavMeshAgent agent;
    public Vector3 EnemyDestination;
    private float timeLimit;
    private float timeCouting;
    private Vector3 positionToAttack;
    public int Level;
    public CharacterName enemyName;
    private IState currentState;

    #endregion Parameter

    private void Awake()
    {
        enemyName = (CharacterName)Random.Range(0, 16);
        IdentifyTypeOfClother((ClotherType)Random.Range(0, 24));
    }

    private void Start()
    {
        GameManager.Instance.CharacterList.Add(this); //Các enemy được sinh ra sẽ được Add vào trong CharacterList này để sử dụng và quản lí states.
        //OnInit(); : đã khởi tạo từ khi spawn nên không cần gọi lại hàm này!
    }

    public override void OnInit()
    {
        base.OnInit();
        AttackRange = 5f;
        AttackSpeed = 10;
        WeaponCreates(); //Khởi tạo danh sách vũ khí
        weaponSwitching((weaponType)Random.Range((int)weaponType.Arrow, (int)weaponType.Z));   //Đổi vũ khí và Material của vũ khí vào
        AddPowerStats();
        IsDeath = false;
        Level = 0;
        ChangeState(new StateEnemyIdle());
    }

    private void Update()
    {
        timeCouting += Time.deltaTime;
        if (!IsDeath && GameManager.Instance.gameState == GameManager.GameState.gameStarted)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        }
    }

    public void EnemyMovement()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.gameStarted)
        {
            agent.SetDestination(EnemyDestination);
            enableToAttackFlag = true;
        }
    }

    public void EnemyStopMoving()
    {
        agent.SetDestination(transform.position);
    }

    public void FindNextDestination()
    {
        EnemyDestination = new Vector3(Random.Range(-24f, 24f), 0, Random.Range(-18.5f, 18.5f)); //Find the random position
    }

    public void CheckArriveDestination()
    {
        if (Vector3.Distance(transform.position, EnemyDestination) < 0.3f)
        {
            ChangeState(new StateEnemyIdle());
        }
    }

    public void ChangeState(IState state)   //Hàm chuyển đổi trạng thái State
    {
        if (state != currentState)
        {
            if (currentState != null)
            {
                currentState.OnExit(this);
            }
            currentState = state;
            if (currentState != null)
            {
                currentState.OnEnter(this);
            }
        }
    }

    public void RestartTimeCounting()
    {
        timeCouting = 0;
        timeLimit = Random.Range(1.5f, 3.5f);
    }

    public void CheckIdletoAttack()
    {
        if (FindNearistEnemy(AttackRange, whatIsPlayer) != Vector3.zero && enableToAttackFlag) ChangeState(new StateEnemyAttack());
    }

    public void CheckPatroltoAttack()
    {
        if (FindNearistEnemy(AttackRange, whatIsPlayer) != Vector3.zero && enableToAttackFlag && timeCouting > 2f) ChangeState(new StateEnemyAttack());
    }

    public void CheckIfAttackIsDone()
    {
        if (enemyAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 || timeCouting > 1.03f)
        {
            ChangeState(new StateEnemyIdle());
        }
    }

    public void CheckIdletoPatrol()
    {
        if (timeCouting > timeLimit)
        {
            ChangeState(new StateEnemyPatrol());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ChangeState(new StateEnemyPatrol());
        }
    }

    public override void Attack()
    {
        transform.LookAt(FindNearistEnemy(AttackRange, whatIsPlayer));
        enableToAttackFlag = false;
        attackScript.SetID(gameObject.GetInstanceID(), opponentID);
    }

    public void OnHit()
    {
        PlayDieAudio();
        currentState.OnExit(this);
        IsDeath = true;
        agent.SetDestination(transform.position);
        GameManager.Instance.KilledAmount++;
        ChangeAnim(CacheString.ANIM_DEATH);
        StartCoroutine(EnemyDeath());
    }

    private IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(2f);
        Pooling.instance._Push(gameObject.tag, gameObject);
    }

    public override void AddLevel()
    {
        characterCanvasAnim.SetTrigger("AddLevel");
        Level++;
        // add stats when kill other character.
        transform.localScale = new Vector3(1f + 0.1f * Level, 1f + 0.1f * Level, 1f + 0.1f * Level);
        agent.speed = (1f + 0.05f * Level) * 5f;
        AttackRange = 1.05f * AttackRange;
    }

    public void weaponSwitching(weaponType _weaponType)
    {
        for (int i = 0; i < weaponArray.Count; i++)
        {
            if (i == (int)_weaponType)
            {
                Material[] CurrentWeaponMaterial = CacheComponents<Renderer>.Get(weaponArray[i]).sharedMaterials;
                Material temp = GetRandomWeaponMaterial(_weaponType);
                for (int j = 0; j < CacheComponents<Renderer>.Get(weaponArray[i]).sharedMaterials.Length; j++)
                {
                    CurrentWeaponMaterial[j] = temp;
                }
                CacheComponents<Renderer>.Get(weaponArray[i]).sharedMaterials = CurrentWeaponMaterial;
                weaponArray[i].SetActive(true);
            }
            else
            {
                weaponArray[i].SetActive(false);
            }
        }
    }

    #region Get Random Weapon Material

    public Material GetRandomWeaponMaterial(weaponType _weaponType)
    {
        for (int i = 0; i < ListWeaponMaterial.Count; i++)
        {
            if (ListWeaponMaterial.ContainsKey(_weaponType))
            {
                int randomMaterialIndex = Random.Range(0, ListWeaponMaterial[_weaponType].Length);
                return ListWeaponMaterial[_weaponType][randomMaterialIndex];
            }
        }
        return ListWeaponMaterial[_weaponType][0];
    }

    #endregion Get Random Weapon Material
}