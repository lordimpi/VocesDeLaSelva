using UnityEngine;

public enum ItemEffectType
{
    None,
    RestoreSanity
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Survival/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [Header("Efecto al usar")]
    public ItemEffectType effectType = ItemEffectType.None;
    public float effectValue = 0f;
}