using UnityEngine;

public class HubArtifact : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject emptyFrame;
    [SerializeField] private GameObject completedMap;

    private bool isCompleted = false;

    public void Interact()
    {
        if (isCompleted) return;

        if (InventoryManager.Instance.AddItem(itemData))
        {
            AudioManager.Instance.PlayObjectMusic(itemData.objectTrack);

            map.SetActive(false);
            emptyFrame.SetActive(true);

            GetComponent<Collider>().enabled = false;
        }
    }

    public void CompleteArtifact()
    {
        isCompleted = true;
        GetComponent<Collider>().enabled = false;
        completedMap.SetActive(true);
    }
}