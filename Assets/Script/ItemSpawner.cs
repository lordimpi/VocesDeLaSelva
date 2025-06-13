using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
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

        while (activeItems.Count < numberOfItems)
        {
            Vector3 position = GetRandomPointOnTerrain();
            GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
            activeItems.Add(newItem);
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
    GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
    
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
