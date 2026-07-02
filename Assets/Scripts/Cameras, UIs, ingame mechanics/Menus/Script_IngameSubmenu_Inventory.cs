using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections;

public class Script_IngameSubmenu_Inventory : Script_IngameSubmenu
{
    private int inventorySize;
    private int optionIndex = 0;
    private TMP_Text[] itemList;

    // Description box
    public GameObject descBox;
    public TMP_Text descNameText;
    public TMP_Text descBodyText;
    public float minBoxHeight = 80f;
    public RectTransform descBoxRect;
    public Vector2 descBoxOffset = new Vector2(20f, 0f);

    public static Script_IngameSubmenu_Inventory instance;

    void Awake() {
        instance = this;
        itemList = GetComponentsInChildren<TMP_Text>();
        foreach (var item in itemList) {
            item.gameObject.SetActive(false);
        }

        descBox.GetComponent<CanvasGroup>().alpha = 0;
        descBox.SetActive(false);
    }

    void Start() {
        inventorySize = Script_PlayerInventory.inventorySize;

        // Defaults to selecting first option first
        HighlightOption(optionIndex);
        enabled = false;
    }

    void Update() {
        // Alternating between menu options
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            MoveSelection(inventorySize / 5);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            MoveSelection(inventorySize - inventorySize / 5);
        }

        // Confirming option
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
            ChooseOption();
        }
    }

    public override void ChooseOption() {
        if (Script_PlayerInventory.inventory[optionIndex] != null) 
            DisplayItemDesc();
    }

    public override void HighlightOption(int index) {
        for (int i = 0; i < inventorySize; i++) {
            var text = itemList[i].GetComponentInChildren<TMP_Text>();
            text.color = (i == index) ? Color.yellow : Color.white;
        }
    }

    public void ShowItem() {
        for (int i = 0; i < inventorySize; i++) {
            var item = Script_PlayerInventory.inventory[i];
            if (item != null) {
                itemList[i].GetComponent<TMP_Text>().text = item.itemNameAbbre;
                itemList[i].gameObject.SetActive(true);
            }
        }
    }

    // Allows selecting non-null slots only
    void MoveSelection(int step) {
        int start = optionIndex;

        // Jumps until it reaches a non-null slot or returns to the starting slot, then highlights that slot
        // If the entire inventory is empty, the bool below returns false, breaking the loop and preventing infinite jumping
        do {
            optionIndex = (optionIndex + step + inventorySize) % inventorySize;
        } while (Script_PlayerInventory.inventory[optionIndex] == null && optionIndex != start);
        HighlightOption(optionIndex);
    }

    public void DisplayItemDesc() {
        var item = Script_PlayerInventory.inventory[optionIndex];
        if (item == null)
            return;

        if (descBox.activeSelf) {
            descBox.SetActive(false);
            descBox.GetComponent<CanvasGroup>().alpha = 0;
            return;
        }

        descNameText.text = item.itemName;
        descBodyText.text = item.itemDesc;
        PositionDescBox();
        descBox.SetActive(true);
        descBox.GetComponent<CanvasGroup>().alpha = 1;

        StartCoroutine(ClampMinHeight());
    }
    private IEnumerator ClampMinHeight() {
        yield return new WaitForEndOfFrame(); // lets ContentSizeFitter recalculate first
        var rt = descBox.GetComponent<RectTransform>();
        float h = rt.sizeDelta.y;
        if (h < minBoxHeight) {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, minBoxHeight);
        }
    }
    private void PositionDescBox() {
        RectTransform selectedRect = itemList[optionIndex].rectTransform;
        descBoxRect.position = selectedRect.position + (Vector3)descBoxOffset;
    }

}
