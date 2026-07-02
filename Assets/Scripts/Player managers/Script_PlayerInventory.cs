using UnityEngine;

public class Script_PlayerInventory : MonoBehaviour
{
    public static Script_PlayerInventory instance;
    public static int inventorySize = 15;

    public static Script_ItemBase[] inventory = new Script_ItemBase[inventorySize];

    void Awake() {
        instance = this;
    }

    public void AddItem(Script_ItemBase item) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == null) {
                inventory[i] = item;
                return;
            }
        }
    }
}
