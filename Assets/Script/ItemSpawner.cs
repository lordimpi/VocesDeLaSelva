using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
    public GameObject prefab;
    public int cantidad = 1;    
    }
    public List<SpawnableItem> objetosASpawnear;
    public Terrain terrain;

    public int numberOfItems = 15;
    public float respawnTime = 10f;

    private List<GameObject> activeItems = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(ManageItems), 0f, respawnTime);
    }

    void ManageItems()
{
    CleanupItems();

    foreach (var objeto in objetosASpawnear)
    {
        int objetosExistentes = activeItems.FindAll(obj => obj != null && obj.name.Contains(objeto.prefab.name)).Count;

        for (int i = objetosExistentes; i < objeto.cantidad; i++)
        {
            Vector3 position = GetRandomPointOnTerrain();
            GameObject newItem = Instantiate(objeto.prefab, position, Quaternion.identity);
            newItem.name = objeto.prefab.name; // útil para identificar en Cleanup

            PickupItem pickup = newItem.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.spawner = this;
            }

            activeItems.Add(newItem);
        }
    }
}


    void CleanupItems()
    {
        activeItems.RemoveAll(item => item == null);
    }

    Vector3 GetRandomPointOnTerrain()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        float x = Random.Range(0, terrainWidth);
        float z = Random.Range(0, terrainLength);
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) + 0.5f;

        return new Vector3(x, y, z);
    }

    public void RespawnItem(Item itemData)
    {
        StartCoroutine(RespawnAfterDelay(itemData));
    }

    private System.Collections.IEnumerator RespawnAfterDelay(Item itemData)
{
    yield return new WaitForSeconds(respawnTime);

    Vector3 position = GetRandomPointOnTerrain();
    GameObject prefab = objetosASpawnear.Find(obj => obj.prefab.GetComponent<PickupItem>().itemData.itemName == itemData.itemName)?.prefab;
    if (prefab == null) yield break;

    GameObject newItem = Instantiate(prefab, position, Quaternion.identity);


    
    // Asignar el itemData y la referencia al spawner
    PickupItem pickup = newItem.GetComponent<PickupItem>();
    if (pickup != null)
    {
        pickup.itemData = itemData;
        pickup.spawner = this;
    }

    activeItems.Add(newItem);
    Debug.Log("Objeto reapareció: " + itemData.itemName);
}
}
