using UnityEngine;

[CreateAssetMenu(fileName = "New Clothes", menuName = "Scriptable Objects/Clothes")]
public class ClothesInfo : ScriptableObject
{
    [Header("Position to Spawn")]
    public ClotherPosition SpawnPosition;

    [Header("Head Prefab")]
    public GameObject HeadPrefab;

    [Header("Left Hand Prefab")]
    public GameObject LeftHandPrefab;

    [Header("Back Prefab")]
    public GameObject BackPrefab;

    [Header("Pan Material")]
    public Material PanMaterial;

    [Header("Skin Material")]
    public Material SkinMaterial;
}