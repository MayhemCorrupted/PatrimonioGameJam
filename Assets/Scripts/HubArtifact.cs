using UnityEngine;

public class HubArtifact : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    [SerializeField] private GameObject artifactMesh; 
    [SerializeField] private GameObject emptyFrameMesh; 
    [SerializeField] private GameObject glowEffect; 

    public void Interact()
    {
        if (InventoryManager.Instance.AddItem(itemData))
        {
            AudioManager.Instance.PlayObjectMusic(itemData.objectTrack);

            artifactMesh.SetActive(false);
            emptyFrameMesh.SetActive(true);

            GetComponent<Collider>().enabled = false;
        }
    }

    public void CompleteArtifact()
    {
        glowEffect.SetActive(true);
    }
}