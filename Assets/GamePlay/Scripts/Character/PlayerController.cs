using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Character, IHit
{
    public static PlayerController instance;
    [SerializeField] private GameObject rangeAttack; //range attack of Player
    [SerializeField] private GameObject Reticle; // target attack
    [SerializeField] private Material[] CupMaterial;
    [SerializeField] private LayerMask whatIsEnemy;
    private Vector3 positionToAttack;
    public int Level;
    public CharacterName KillerName;
    private IStatePlayer currentState;
    private Vector3 dir;

    // Start is called before the first frame update
    private void Start()
    {
        Joystick.OnMove -= GetDirection;
        Joystick.OnMove += GetDirection;
        GameManager.Instance.CharacterList.Add(this);  //Thêm player vào trong list Character để quản lý
        //OnInit();
    }

    public override void OnInit()
    {
        base.OnInit();
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 6f;
        WeaponCreates();                 //Khởi tạo danh sách vũ khí
        CreateListOfWeaponMaterial();       //Khởi tạo danh sách Material của vũ khí
        weaponSwitching(weaponType.Hammer, new WeaponMaterialsType[] { WeaponMaterialsType.Hammer_1 });
        UpdatePlayerItem();
        changeAttackRange(AttackRange);
        IsDeath = false;
        Level = 0;
        ChangeState(new StatePlayerIdle());
    }

    // Update is called once per frame
    private void Update()
    {
        ShowReticle();
        ObstacleFading();
        if (!IsDeath && GameManager.Instance.gameState == GameManager.GameState.gameStarted)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        }
        else if (GameManager.Instance.gameState == GameManager.GameState.gameWin)
        {
            ChangeAnim(CacheString.ANIM_WIN);
        }
        rangeAttack.transform.position = transform.position;
        if (GameManager.Instance.IsAliveAmount == 1 && !IsDeath) StartCoroutine(CheckGameVictory());
    }

    private void GetDirection(Vector2 direction)
    {
        dir = direction != Vector2.zero ? direction.normalized : Vector3.zero;
    }

    public override void Move()
    {
        Vector3 newDir = new Vector3(-dir.y, 0, dir.x);
        if (GameManager.Instance.gameState == GameManager.GameState.gameStarted)
        {
            if (newDir != Vector3.zero)       //Nếu vị trí joystick được di chuyển thì Move Player
            {
                Quaternion toRotate = Quaternion.LookRotation(newDir.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, 720 * Time.deltaTime);
                transform.position += newDir * (Time.deltaTime * MoveSpeed);
                enableToAttackFlag = true;      //Bật cờ Attack để khi Character dừng lại thì sẽ tấn công nếu có Enemy ở trong bán kính tấn công.
            }
        }
    }

    public void CheckIdleToPatrol()
    {
        if (dir != Vector3.zero && !IsDeath)
        {
            ChangeState(new StatePlayerPatrol());
        }
    }

    public void CheckPatrolToIdle()
    {
        if (dir == Vector3.zero && !IsDeath) ChangeState(new StatePlayerIdle());
    }

    public void CheckIdletoAttack()
    {
        if (enableToAttackFlag && FindNearistEnemy(AttackRange, whatIsEnemy) != Vector3.zero && !IsDeath)
        {
            ChangeState(new StatePlayerAttack());
        }
    }

    public override void Attack()
    {
        transform.LookAt(positionToAttack);
        enableToAttackFlag = false;
        attackScript.SetID(gameObject.GetInstanceID(), opponentID);
        StartCoroutine(TurntoIdle());
    }

    private IEnumerator TurntoIdle()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.gameState == GameManager.GameState.gameStarted && dir != Vector3.zero && !IsDeath) ChangeState(new StatePlayerIdle());
    }

    private void changeAttackRange(float attackRange)
    {
        AttackRange = attackRange;
        rangeAttack.transform.localScale = new Vector3(AttackRange, 1f, AttackRange);
    }

    #region Reticle

    private void ShowReticle() //Hiện mục tiêu của Player
    {
        positionToAttack = FindNearistEnemy(AttackRange, whatIsEnemy);
        if (positionToAttack != Vector3.zero)
        {
            Reticle.transform.position = new Vector3(positionToAttack.x, 0.1f, positionToAttack.z);
            Reticle.SetActive(true);
        }
        else
        {
            Reticle.SetActive(false);
        }
    }

    #endregion Reticle

    #region Singleton

    //private void IInitializeSingleton()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    #endregion Singleton

    public void OnHit()
    {
        currentState.OnExit(this);
        ChangeAnim(CacheString.ANIM_DEATH);
        Reticle.SetActive(false);
        IsDeath = true;
        GameManager.Instance.KilledAmount++;
        GameManager.Instance.gameState = GameManager.GameState.gameOver; // ui canvas gameover
        PlayDieAudio();
    }

    private void ObstacleFading()
    {
        //foreach (Transform _obstacle in GameManager.Instance.Obstacle)
        //{
        //    if (Vector3.Distance(transform.position, _obstacle.position) < 8f)
        //    {
        //        _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[1];
        //    }
        //    else
        //    {
        //        _obstacle.GetComponent<Renderer>().sharedMaterial = CupMaterial[0];
        //    }
        //}
    }

    public override void AddLevel()                                                                     //Mỗi lần bắn hạ đối thủ thì sẽ gọi hàm AddLevel
    {
        characterCanvasAnim.SetTrigger("AddLevel");                                                     //Chạy Anim +1 khi giết được 1 enemy
        Level++;
        transform.localScale = new Vector3(1f + 0.1f * Level, 1f + 0.1f * Level, 1f + 0.1f * Level);    //Khi tăng 1 level thì sẽ tăng Scale của Player thêm 10% so với kích thước khi Start game
        MoveSpeed = (1f + 0.05f * Level) * 5f;                                                          //Tốc độ di chuyển của Player tăng 5% so với khi Start game.
        changeAttackRange(1.05f * AttackRange);                                                         //Tăng 5% tầm bắn
        PlaySizeUpAudio();
    }

    public void weaponSwitching(weaponType _weaponType, WeaponMaterialsType[] _weaponMaterial)
    {
        AttackRange = 5f;
        AttackSpeed = 10;
        MoveSpeed = 5f;
        Material[] CurrentWeaponMaterial = new Material[_weaponMaterial.Length];
        for (int i = 0; i < weaponArray.Count; i++)
        {
            if (i == (int)_weaponType)
            {
                CurrentWeaponMaterial = CacheComponents<Renderer>.Get(weaponArray[i]).sharedMaterials;
                //TODO: change material of weapon
                //CurrentWeaponMaterial = GetWeaponMaterial(_weaponType);
                CacheComponents<Renderer>.Get(weaponArray[i]).sharedMaterials = CurrentWeaponMaterial;
                weaponArray[i].SetActive(true);
            }
            else
            {
                weaponArray[i].SetActive(false);
            }
        }
        AddPowerStats();
    }

    //private Material[] GetWeaponMaterial(weaponType _weaponType)
    //{
    //    int countMaterial =
    //    Material[] desireMaterials = new Material[_weaponMaterial.Length];
    //    if (_weaponMaterial.Length == 1)
    //    {
    //        Material[] desireMaterial = ListWeaponMaterial[_weaponMaterial][0];
    //        return desireMaterial;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < _weaponMaterial.Length; i++)
    //        {
    //            desireMaterials[i] = ListWeaponMaterial[_weaponMaterial[i]][0];
    //        }
    //    }
    //    return desireMaterials;
    //}

    public void ChangeState(IStatePlayer state)
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

    public void UpdatePlayerItem()  //Trang bị clothes và weapon cho Player khi tắt đi bật lại
    {
        for (int i = 0; i < 12; i++)
        {
            if (PlayerPrefs.GetInt("WeaponShop" + (weaponType)i) == 4)
            {
                weaponSwitching((weaponType)i, new WeaponMaterialsType[] { WeaponMaterialsType.Arrow });
            }
        }
        //for (int i = 0; i < ListClothers.Count; i++)
        //{
        //    if (PlayerPrefs.GetInt("ClothesShop" + (ClotherType)i) == 4)
        //    {
        //        IdentifyTypeOfClother((ClotherType)i);
        //    }
        //}
    }

    private IEnumerator CheckGameVictory()
    {
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.IsAliveAmount == 1 && !IsDeath)
        {
            GameManager.Instance.gameState = GameManager.GameState.gameWin;    //Chỉ còn 1 character còn sống và Player vẫn sống thì Victory
            PlayWinAudio();
        }
        //else
        //{ GameManager.Instance.gameState = GameManager.GameState.gameOver; }
    }
}