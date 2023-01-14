using UnityEngine;

[CreateAssetMenu(fileName = "StoreItem", menuName = "ScriptableObject/StoreItem")]
public class StoreItem : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public int price;
    public bool hasBought;
    public GameObject itemPrefab;
    public string tipoItem;
}
