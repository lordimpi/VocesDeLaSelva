using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public Transform slotContainer;
    public GameObject slotPrefab;

    public GameObject accionesFlotantes;
    public Button comerButton;
    private Item itemSeleccionado;

    private float lastClickTime = 0f;
private float doubleClickThreshold = 0.3f; // segundos para considerar doble clic


    private List<GameObject> slotsVisuales = new List<GameObject>();

    public void RefreshInventory()
    {
        // Limpiar anteriores
        foreach (GameObject slot in slotsVisuales)
            Destroy(slot);
        slotsVisuales.Clear();

        // Crear nuevos slots
        foreach (Item item in playerInventory.items)
        {
            GameObject nuevoSlot = Instantiate(slotPrefab, slotContainer);
            Image imagen = nuevoSlot.GetComponent<Image>();
            if (imagen != null)
                imagen.sprite = item.icon;

            // Asignar evento de clic
            Button boton = nuevoSlot.GetComponent<Button>();
            if (boton != null)
            {
                Item itemCopia = item;
                // Añadir listener para clics
                boton.onClick.AddListener(() => OnItemClicked(itemCopia, nuevoSlot));
            }

            slotsVisuales.Add(nuevoSlot);
        }

        accionesFlotantes.SetActive(false); // ocultar panel por si acaso
    }

    public void MostrarAcciones(Item item, GameObject slotVisual)
    {
        itemSeleccionado = item;

        accionesFlotantes.SetActive(true);

        // Posicionar el panel encima del ítem
        RectTransform panelRT = accionesFlotantes.GetComponent<RectTransform>();
        RectTransform slotRT = slotVisual.GetComponent<RectTransform>();

        // Convertir la posición del slot al espacio de pantalla
        Vector3[] corners = new Vector3[4];
        slotRT.GetWorldCorners(corners);
        Vector3 position = corners[2]; // esquina superior derecha

        panelRT.position = position;
    }

    public void ComerItem()
    {
        if (itemSeleccionado != null)
        {
            Debug.Log("Comiste: " + itemSeleccionado.itemName);
            playerInventory.RemoveItem(itemSeleccionado);
            itemSeleccionado = null;
            accionesFlotantes.SetActive(false);
            RefreshInventory();
        }
    }

    public void EliminarItem()
    {
        if (itemSeleccionado != null)
        {
            Debug.Log("Eliminaste: " + itemSeleccionado.itemName);
            playerInventory.RemoveItem(itemSeleccionado);
            itemSeleccionado = null;
            accionesFlotantes.SetActive(false);
            RefreshInventory();
        }
    }

    public void OnItemClicked(Item item, GameObject slotVisual)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            // Doble clic: comer
            itemSeleccionado = item;
            ComerItem();
        }
        else
        {
            // Clic normal: mostrar botón de eliminar
            MostrarAcciones(item, slotVisual);
        }

        lastClickTime = Time.time;
    }

}
