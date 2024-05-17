using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRandomSkin", menuName = "Scriptable Objects/Skin")]
public class EnemyRandomSkin : ScriptableObject
{
    [Header("Skin Color")]
    public List<Material> EnemyColor;
}
