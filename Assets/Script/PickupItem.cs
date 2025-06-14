using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item itemData;
    private bool playerInRange = false;
    private Inventory playerInventory;
    public GameObject pickupCanvas;
    public ItemSpawner spawner;

    void Start()
    {
        if (pickupCanvas != null)
            pickupCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                bool added = playerInventory.AddItem(itemData);
                if (added)
                {
                    Debug.Log("Recogiste: " + itemData.itemName);

                    if (spawner != null)
                    {
                        spawner.RespawnItem(itemData);
                    }
                    
                    InventoryUI ui = FindFirstObjectByType<InventoryUI>();
                    if (ui != null)
                    {
                        ui.RefreshInventory();
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<Inventory>();

            if (pickupCanvas != null)
                pickupCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;

            if (pickupCanvas != null)
                pickupCanvas.SetActive(false);
        }
    }
}
