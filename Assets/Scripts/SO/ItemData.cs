using UnityEngine;

[System.Serializable]
public enum ItemType
{
    None,
    Stamina,
    Sanity,
    Hunger
}

[CreateAssetMenu(menuName = "SO")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemValue = 10;
    public ItemType itemType = ItemType.None;
}


