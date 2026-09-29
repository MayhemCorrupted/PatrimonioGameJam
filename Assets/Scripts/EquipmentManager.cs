using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }
    [SerializeField] Image page;
    GameObject currentEquipedModel;
    ItemData currentData;
    public ItemData CurrentEquippedItem => currentData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EquipItem(ItemData item)
    {
        Unequip();
        currentData = item;
        if (page != null)
        {
            page.color = Color.white;
            page.sprite = item.icon;
            currentEquipedModel = new GameObject(item.itemName);
        }
    }
    public void Unequip()
    {
        if (currentData != null)
        {
            if (page != null)
            {
                page.color = Color.white;
                page.sprite = null;
            }

            if (currentEquipedModel != null)
            {
                currentEquipedModel = null;
            }
            currentData = null;
        }
    }
}