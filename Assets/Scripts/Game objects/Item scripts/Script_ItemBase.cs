using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Script_ItemBase : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemNameAbbre;
    public string itemDesc;
}
