using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;

    [Header("Audio (Gestor Musical)")]
    public AudioClip objectTrack;
    public AudioClip pingSound;
}
