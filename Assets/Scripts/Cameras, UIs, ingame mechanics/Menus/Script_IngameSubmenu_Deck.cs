using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Script_IngameSubmenu_Deck : Script_IngameSubmenu 
{
    // The actual pool of all card prefabs.
    // Technically, it tracks its cards by optionIndex, so logically it doesn't use allCardsList at all.
    public SC_DeckInfo[] allCards;
    // Displays images
    public UnityEngine.UI.Image[] allCardsList;
    public UnityEngine.UI.Image[] battleCardList;
    public Sprite[] elementFrames;

    private bool inAllCardsList = true;
    private int optionIndex = 0;
    public static Script_IngameSubmenu_Deck instance;

    // The index a card will take when put into the battle deck;
    // it increments as the battle deck is filled
    private int battleDeckIndex = 0;

    void Awake() {
        instance = this;
        foreach (var item in allCardsList) {
            item.gameObject.SetActive(false);
        }
        foreach (var item in battleCardList) {
            item.gameObject.SetActive(false);
        }
    }

    void Start() {
        // Sets the sprites
        for (int i = 0; i < allCards.Length; i++) {
            allCardsList[i].sprite = allCards[i].scSprite;
            SetElementalFrame(allCardsList, i, allCards[i].scElement);
        }

        // Defaults to selecting first option first
        HighlightOption(optionIndex);
        enabled = false;
    }

    public override void ChooseOption() {
        if (inAllCardsList)
            Choose_AllCardsList();
    }

    public void Choose_AllCardsList() {
        Equip();
    }
    void DisplayItemDesc() {

    }

    void Equip() {
        if (battleDeckIndex >= battleCardList.Length) // Deck full
            return;

        DeckManager.instance.battleDeck.Add(allCards[optionIndex].scPrefab);
        battleCardList[battleDeckIndex].sprite = allCards[optionIndex].scSprite;
        SetElementalFrame(battleCardList, battleDeckIndex, allCards[optionIndex].scElement);

        battleCardList[battleDeckIndex].gameObject.SetActive(true);
        battleDeckIndex++;
    }

    // In order: Metal, Wood, Water, Fire, Earth
    // Has to follow the same order as ENUM_ElementType
    void SetElementalFrame(UnityEngine.UI.Image[] cardList, int i, ElementType element) {
        cardList[i].GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite =
            elementFrames[(int)element];
    }

    public override void HighlightOption(int index) {
        for (int i = 0; i < 12; i++) {
            allCardsList[i].transform.localScale = (i == index) ? new Vector2(1.3f, 1.3f) : new Vector2(1, 1);
        }
    }

    void MoveSelection(int step) {
        optionIndex = (optionIndex + step + 12) % 12;
        HighlightOption(optionIndex);
    }

    void Update() {
        // Alternating between menu options
        if (inAllCardsList) {        
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                MoveSelection(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                MoveSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                MoveSelection(4);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                MoveSelection(8);
            }

            // Confirming option
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
                ChooseOption();
            }
        }
    }

    public void ShowItem() {
        for (int i = 0; i < 12; i++) {
            allCardsList[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < 8; i++) {
            battleCardList[i].gameObject.SetActive(true);
        }
    }
}
