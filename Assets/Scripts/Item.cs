using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable
{
    [Header("Data")]
    public ItemData itemData;
    [SerializeField] bool isPickable = true;
    [SerializeField] UnityEvent OnPicked;
    [SerializeField] UnityEvent OnUnpickable;
    public bool IsPickable { set { isPickable = value; } }
    #region referencias de la interfaz (el IInteractable)
    public void Interact()
    {
        PickUp();
    }
    #endregion
    public void PickUp()
    {
        if (!isPickable)
        {
            OnUnpickable?.Invoke();
            return;
        }
        if (itemData == null)
        {
            Debug.LogError($"[Item] ItemData no asignado en '{gameObject.name}'.");
            return;
        }
        bool pickedUp = InventoryManager.Instance.AddItem(itemData);

        if (pickedUp)
        {
            OnPicked?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
