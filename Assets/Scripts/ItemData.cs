using UnityEngine;

public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    [Space(10)]
    [Header("Item Section")]
    [TextArea] public string description;
    public Sprite sprite;
    public GameObject itemModelPrefab;
}
