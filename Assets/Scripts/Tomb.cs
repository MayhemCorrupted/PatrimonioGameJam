using UnityEngine;

public class Tomb : MonoBehaviour, IInteractable
{
    [SerializeField] private int requiredItemId;
    [SerializeField] private HubArtifact linkedHubArtifact; 
    [SerializeField] private PingIndicator pingSystem;
    public void Interact()
    {
        ItemData currentItem = EquipmentManager.Instance.CurrentEquippedItem;

        if (currentItem != null && currentItem.id == requiredItemId)
        {
            InventoryManager.Instance.RemoveItem(currentItem);

            AudioManager.Instance.PlayBaseMusic();

            GameManager.Instance.AddDeliveredObject();
            if (linkedHubArtifact != null) linkedHubArtifact.CompleteArtifact();

            if (pingSystem != null) pingSystem.ResetPing();

            GetComponent<Collider>().enabled = false;
        }
    }
}