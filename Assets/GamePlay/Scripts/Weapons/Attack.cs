using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private Transform enemyPosition;
    private int OwnerID, OpponentID;

    private void _HideWeapon()
    {
        foreach (Transform weapon in weaponPosition)
        {
            CacheComponents<MeshRenderer>.Get(weapon.gameObject).enabled = false;
            Material[] BulletMaterial = weapon.GetComponent<Renderer>().sharedMaterials;
            if (weapon.gameObject.activeSelf)
            {
                CacheComponents<BulletSpawner>.Get(weapon.gameObject).CreateBullet(weaponPosition.position, OwnerID, OpponentID, BulletMaterial);
            }
        }
        StartCoroutine(_ShowWeapon(0.18f));
    }

    private IEnumerator _ShowWeapon(float _timeCounting)
    {
        yield return new WaitForSeconds(_timeCounting);
        foreach (Transform weapon in weaponPosition)
        {
            CacheComponents<MeshRenderer>.Get(weapon.gameObject).enabled = true;
        }
    }

    public void SetID(int _ownerID, int _opponentID)
    {
        OwnerID = _ownerID;
        OpponentID = _opponentID;
    }
}