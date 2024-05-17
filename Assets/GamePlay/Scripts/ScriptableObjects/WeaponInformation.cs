using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfor_", menuName = "Scriptable Objects/Weapon Information")]
public class WeaponInformation : ScriptableObject
{
    [Header("Weapon Prefab")]
    public GameObject WeaponPrefab;

    [Header("Attack Range Stat")]
    public float AttackRange;

    [Header("Attack Speed Stat")]
    public float AttackSpeed;

    [Header("Materials")]
    public Material[] Materials;
}