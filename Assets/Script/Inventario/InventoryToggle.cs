using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryUI;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryUI.SetActive(isOpen);

            // Opcional: detener el tiempo del juego mientras está abierto
            // Time.timeScale = isOpen ? 0f : 1f;
        }
    }
}
