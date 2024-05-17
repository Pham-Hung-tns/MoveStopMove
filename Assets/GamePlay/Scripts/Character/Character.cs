using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    #region enum

    [HideInInspector] public enum CharacterName { ABI, Uniqlo, Bitis, Vinamilk, KoBaYaShi, Ford, Vinfast, ToYoTa, Yamato, Biden, Biladen, Vodka, Yamaha, Honda, Suzuki, NiShiNo, Furuki }

    [HideInInspector] public enum weaponType { Arrow, Axe_0, Axe_1, boomerang, candy_0, candy_1, candy_2, candy_4, Hammer, knife, uzi, Z }

    [HideInInspector] public enum AddPowerType { addition, multiplication }

    [HideInInspector]
    public enum WeaponMaterialsType
    {
        Arrow,
        Axe0, Axe0_2,
        Axe1, Axe1_2,
        Boomerang_1, Boomerang_2,
        Candy0_1, Candy0_2,
        Candy1_1, candy1_2, candy1_3, candy1_4,
        Candy2_1, Candy2_2,
        Candy4_1, Candy4_2,
        Hammer_1, Hammer_2,
        Knife_1, Knife_2,
        Uzi_1, Uzi_2,
        Z,
        Azure, Black,
        Blue, Chartreuse,
        Cyan, Green,
        Magenta, Orange,
        Red, Rose,
        SpringGreen, Violet,
        White, Yellow
    }

    [HideInInspector]
    public enum SetFullOrNormal
    { SetFull, Normal }

    #endregion enum

    public SetFullOrNormal lastClothes;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip DieAudio;
    [SerializeField] private AudioClip SizeUpAudio;
    [SerializeField] private AudioClip WinAudio;

    private string currentAnim;

    public Attack attackScript;
    public float AttackRange;
    public float AttackSpeed;
    public float MoveSpeed;
    public ClothesInfo[] CharacterClothes;
    public ClothesPower clothesPower; // stats of clothers
    public Transform ShieldPosition;
    public Transform LeftHandPosition;
    public Transform HeadPosition;
    public Transform TailPosition;
    public Transform BackPosition;
    public Renderer PantsPositionRenderer; // This GameObject hold material that is used to change pant
    public Renderer SkinPositionRenderer; // This GameObject hold material that is used to change skin
    public Transform[] weaponPositions;                        //Position that spawn weapons.
    public List<GameObject> weaponArray = new List<GameObject>();   // array weapons of character
    public Animator characterCanvasAnim;
    public WeaponInformation[] _weaponInfo; // current weapon
    public EnemyRandomSkin _enemySkin; // skin of enemy -> use random skin method to change this.
    public bool enableToAttackFlag = false;
    public float distanceToNearistEnemy;
    public Vector3 nearistEnemyPosition;
    public int opponentID;
    public int EnemySkinID;
    public bool IsDeath;
    public AudioSource audiosource;

    public static Dictionary<weaponType, Material[]> ListWeaponMaterial = new Dictionary<weaponType, Material[]>();
    public static Dictionary<ClotherType, ClothesInfo> ListClothers = new Dictionary<ClotherType, ClothesInfo>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        PantsPositionRenderer = GetComponent<Renderer>();
        SkinPositionRenderer = GetComponent<Renderer>();
    }

    public virtual void OnInit()
    {
        currentAnim = CacheString.ANIM_IDLE;
        ChangeAnim(currentAnim);
        CreateListOfSkin();
    }

    private void CreateListOfSkin()
    {
        if (ListClothers.Count > 0)
            return;
        foreach (ClotherType name in Enum.GetValues(typeof(ClotherType)))
        {
            // ten - id
            ListClothers.Add(name, CharacterClothes[(int)name]);
        }
    }

    public virtual void Attack()
    {
    }

    public virtual void Move()
    {
    }

    public virtual void AddLevel()
    {
    }

    #region Change Audio

    public void PlayDieAudio()
    {
        if (AudioManager.Instance.OpenSound) audiosource.PlayOneShot(DieAudio);
    }

    public void PlaySizeUpAudio()
    {
        if (AudioManager.Instance.OpenSound) audiosource.PlayOneShot(SizeUpAudio);
    }

    public void PlayWinAudio()
    {
        if (AudioManager.Instance.OpenSound) audiosource.PlayOneShot(WinAudio);
    }

    #endregion Change Audio

    public void WeaponCreates() //add Weapon to weaponList
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            weaponArray.Add(weaponPositions[i].gameObject);
        }
    }

    public Vector3 FindNearistEnemy(float attackRange, LayerMask whatIsLayer)
    {
        distanceToNearistEnemy = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, whatIsLayer);
        if (colliders.Length == 0)
        {
            return Vector3.zero;
        }
        else
        {
            // find position's nearist Enemy.
            for (int i = 0; i < colliders.Length; i++)
            {
                float newDistance = Vector3.Distance(colliders[i].transform.position, transform.position);
                if (newDistance < distanceToNearistEnemy)
                {
                    distanceToNearistEnemy = newDistance;
                    nearistEnemyPosition = colliders[i].transform.position;
                    opponentID = colliders[i].gameObject.GetInstanceID();
                }
            }
        }
        return nearistEnemyPosition;
    }

    //- player sẽ có 2 chỉ số được tăng tiến: AttackRange và AttackSpeed -> tăng theo cấp và
    //loại vũ khí, trang phục đang mặc
    //Đang cầm loại nào thì sẽ cộng thêm AttackRange và AttackSpeed tương ứng vào.
    public void AddPowerStats()
    {
        for (int i = 0; i < weaponArray.Count; i++)
        {
            if (weaponArray[i].activeSelf)
            {
                AttackRange += _weaponInfo[i].AttackRange;
                AttackSpeed += _weaponInfo[i].AttackSpeed;
                break;
            }
        }
    }

    #region Create Dict of Weapon Materials

    public void CreateListOfWeaponMaterial()
    {
        if (ListWeaponMaterial.Count > 0)
            return;
        foreach (weaponType name in Enum.GetValues(typeof(weaponType)))
        {
            // ten - id
            ListWeaponMaterial.Add(name, _weaponInfo[(int)name].Materials);
        }
    }

    #endregion Create Dict of Weapon Materials

    public void ChangeAnim(string newAnim)
    {
        if (currentAnim != null)
        {
            if (currentAnim != newAnim)
            {
                anim.ResetTrigger(currentAnim);
                currentAnim = newAnim;
            }
            anim.SetTrigger(currentAnim);
        }
    }

    private void ChangeHeadClother(ClotherType _ClothesType)
    {
        if (ListClothers[_ClothesType].HeadPrefab == null)
            return;
        ResetHeadPosition();
        Pooling.instance._Pull(ListClothers[_ClothesType].HeadPrefab.name, ListClothers[_ClothesType].HeadPrefab, HeadPosition);
    }

    private void ChangeLeftHandClother(ClotherType _ClothesType)
    {
        if (ListClothers[_ClothesType].LeftHandPrefab == null)
            return;
        ResetLeftHandPosition();
        Pooling.instance._Pull(ListClothers[_ClothesType].LeftHandPrefab.name, ListClothers[_ClothesType].LeftHandPrefab, LeftHandPosition);
    }

    private void ChangePanClother(ClotherType _ClothesType)
    {
        if (ListClothers[_ClothesType].PanMaterial == null)
            return;
        PantsPositionRenderer.sharedMaterial = ListClothers[_ClothesType].PanMaterial;
    }

    private void ChangeBackClother(ClotherType _ClothesType)
    {
        if (ListClothers[_ClothesType].BackPrefab == null)
            return;
        ResetBackPosition();
        Pooling.instance._Pull(ListClothers[_ClothesType].BackPrefab.name, ListClothers[_ClothesType].BackPrefab, BackPosition);
    }

    private void ChangeSkin(ClotherType _ClotherType)
    {
        if (ListClothers[_ClotherType].SkinMaterial == null)
            return;
        SkinPositionRenderer.sharedMaterial = ListClothers[_ClotherType].SkinMaterial;
    }

    private void ChangeSetFull(ClotherType _ClothesType)
    {
        ChangeHeadClother(_ClothesType);
        ChangeBackClother(_ClothesType);
        ChangeLeftHandClother(_ClothesType);
        ChangePanClother(_ClothesType);
        ChangeSkin(_ClothesType);
    }

    #region ChangeClothes

    public void IdentifyTypeOfClother(ClotherType _ClothesType)
    {
        if (ListClothers.ContainsKey(_ClothesType))
        {
            switch (ListClothers[_ClothesType].SpawnPosition)
            {
                case ClotherPosition.Head:
                    {
                        ChangeHeadClother(_ClothesType);
                        break;
                    }
                case ClotherPosition.LeftHand:
                    {
                        ChangeLeftHandClother(_ClothesType);
                        break;
                    }
                case ClotherPosition.Pan:
                    {
                        ChangePanClother(_ClothesType);
                        break;
                    }
                case ClotherPosition.SetFull:
                    {
                        ChangeSetFull(_ClothesType);
                        break;
                    }
            }
        }
    }

    public void ResetClothes()
    {
        ResetShieldPosition();
        ResetLeftHandPosition();
        ResetHeadPosition();
        ResetBackPosition();
        ResetTailPosition();
        GetDefaultClothes();
    }

    public void GetDefaultClothes() //Thay đổi quần và màu skin về default
    {
        //PantsPositionRenderer.sharedMaterial = ListClothers[]PantsMaterials[3];
        //if (gameObject.CompareTag("Player"))
        //    SkinPositionRenderer.sharedMaterial = CharacterClothes.SkinMaterials[1];                    //Reset màu cho Player
        //else                                                        //Nếu là Enemy thì chọn màu random
        //{
        //    EnemySkinID = UnityEngine.Random.Range(0, 21);
        //    SkinPositionRenderer.sharedMaterial = _enemySkin.EnemyColor[EnemySkinID];
        //}
    }

    public void ResetShieldPosition()
    {
        foreach (Transform item in ShieldPosition)
        {
            Pooling.instance._Push(item.gameObject.name, item.gameObject);
        }
    }

    public void ResetLeftHandPosition()
    {
        foreach (Transform item in LeftHandPosition)
        {
            Pooling.instance._Push(item.gameObject.name, item.gameObject);
        }
    }

    public void ResetHeadPosition()
    {
        foreach (Transform item in HeadPosition)
        {
            Pooling.instance._Push(item.gameObject.name, item.gameObject);
        }
    }

    public void ResetBackPosition()
    {
        foreach (Transform item in BackPosition)
        {
            Pooling.instance._Push(item.gameObject.name, item.gameObject);
        }
    }

    public void ResetTailPosition()
    {
        foreach (Transform item in TailPosition)
        {
            Pooling.instance._Push(item.gameObject.name, item.gameObject);
        }
    }

    #endregion ChangeClothes
}