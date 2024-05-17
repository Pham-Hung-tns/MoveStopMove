using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Renderer render;
    private Vector3 TargetPos, OwnerAttackPos;
    private float bulletSpeed;
    private float AttackRange;
    private Rigidbody _bullet;
    private int OwnerID, OpponentID;

    public Renderer Render { get => render; set => render = value; }

    private void Start()
    {
        BulletMove();
    }

    private void OnEnable()
    {
        AudioManager.Instance.PlayAttackAudio();
    }

    private void Update()
    {
        if (Vector3.Distance(OwnerAttackPos, transform.position) > AttackRange)
        {
            DestroyBullet();
        }
    }

    public void BulletMove()
    {
        _bullet = CacheComponents<Rigidbody>.Get(this.gameObject);
        Vector3 dirrect = TargetPos - transform.position;
        _bullet.velocity = dirrect.normalized * bulletSpeed;
        transform.LookAt(TargetPos);
    }

    public void SetID(int _ownerID, int _oppenentID)
    {
        OwnerID = _ownerID;
        OpponentID = _oppenentID;
        GetPower(OwnerID);
        FindTarget();
    }

    private void GetPower(int _ownerID)
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == _ownerID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.CharacterList[i].IsDeath == false)
                {
                    AttackRange = GameManager.Instance.CharacterList[i].AttackRange;
                    bulletSpeed = GameManager.Instance.CharacterList[i].AttackSpeed;
                    transform.localScale = GameManager.Instance.CharacterList[i].gameObject.transform.localScale;
                }
            }
        }
    }

    public void FindTarget()
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OpponentID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                TargetPos = GameManager.Instance.CharacterList[i].gameObject.transform.position;
                TargetPos.y = 1f;
            }
            else if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID)
            {
                OwnerAttackPos = GameManager.Instance.CharacterList[i].gameObject.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() != OwnerID) //Xét xem đối tượng trúng đạn có phải Owner ko?
        {
            if (other.CompareTag("Enemy"))              //Kiểm tra xem đối tượng trúng đạn có còn sống không (Vì đối tượng còn sống lúc Owner bắn đạn nhưng có thể đã trúng đạn và chết trước khi đạn của Owner bắn đến nơi) )
            {
                if (CacheComponents<Character>.Get(other.gameObject).IsDeath == false)
                {
                    other.gameObject.GetComponent<IHit>().OnHit();
                    AddOwnerLevel();
                    DestroyBullet();
                }
            }
            else if (other.CompareTag("Player"))
            {
                if (CacheComponents<Character>.Get(other.gameObject).IsDeath == false)
                {
                    for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
                    {
                        if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID)
                        {
                            if (GameManager.Instance.CharacterList[i].gameObject.CompareTag("Enemy"))
                            {
                                CacheComponents<PlayerController>.Get(other.gameObject).KillerName = CacheComponents<EnemyController>.Get(GameManager.Instance.CharacterList[i].gameObject).enemyName;
                            }
                        }
                    }
                    other.gameObject.GetComponent<IHit>().OnHit();
                    AddOwnerLevel();
                    DestroyBullet();
                }
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            AudioManager.Instance.PlayWeaponImpackSound();
            DestroyBullet();
        }
    }

    private void AddOwnerLevel()
    {
        for (int i = 0; i < GameManager.Instance.CharacterList.Count; i++)
        {
            if (GameManager.Instance.CharacterList[i].gameObject.GetInstanceID() == OwnerID && GameManager.Instance.CharacterList[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.CharacterList[i].IsDeath == false)
                {
                    GameManager.Instance.CharacterList[i].AddLevel();
                }
            }
        }
    }

    private void DestroyBullet()
    {
        _bullet.velocity = Vector3.zero;
        Pooling.instance._Push(gameObject.tag, gameObject);
    }
}