using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Script_IngameMenu : MonoBehaviour
{
    public enum MenuMode {
        Overworld, Battle
    }

    public CanvasGroup canvasGroup;
    public MenuMode menuMode;
    public static bool isMenuActive = false;
    public UnityEngine.UI.Image[] menuOption;

    private Sprite[] baseSprite;
    public Sprite[] highlightSprite;
    private Vector3[] original_pos;

    public CanvasGroup[] submenu;

    public static bool isUsingSubmenu = false;
    private int optionIndex = 0;
    private int NO_OF_OPTIONS = -1;

    // Since menu checks for isPonderingQuitting to show/hide,
    // isPonderingQuitting being false until the quit menu is accessed means menu will show at all times.
    // This bool gates that
    private bool hideForQuitMenu = false;

    void Awake() {
        if (menuMode == MenuMode.Overworld)
            NO_OF_OPTIONS = 4; // Deck, Inventory, Encyclopedia, Settings
        else if (menuMode == MenuMode.Battle)
            NO_OF_OPTIONS = 3; // Deck, Encyclopedia, Settings

        canvasGroup.alpha = 0;
    }

    void Start() {
        original_pos = new Vector3[NO_OF_OPTIONS];
        for (int i = 0; i < NO_OF_OPTIONS; i++) {
            original_pos[i] = menuOption[i].rectTransform.position;
        }

        baseSprite = new Sprite[NO_OF_OPTIONS];
        for (int i = 0; i < NO_OF_OPTIONS; i++) {
            baseSprite[i] = menuOption[i].sprite;
        }
    }

    void Update()
    {
        // Only permits opening menu when not in dialogue (it would be rude otherwise)
        if (!DialogueManager.isDialogueActive) {

            // On key press
            if (Input.GetKeyDown(PLAYER_INPUT.menu)) {

                // If using submenu, turns it off
                if (isUsingSubmenu) {
                    foreach (var menu in submenu) menu.alpha = 0;

                    submenu[optionIndex].GetComponent<Script_IngameSubmenu>().enabled = false;

                    isUsingSubmenu = false;
                }

                // Else turns off the whole menu
                else {
                    canvasGroup.alpha = (canvasGroup.alpha + 1) % 2;
                    isMenuActive = canvasGroup.alpha == 1;
                    Time.timeScale = canvasGroup.alpha == 1 ? 0f : 1f;

                    if (canvasGroup.alpha == 0) {
                        isUsingSubmenu = false;
                        optionIndex = 0;
                    }
                }
            }

            // When menu is active
            if (canvasGroup.alpha == 1) {
                HighlightOption(optionIndex);

                // Disables menu control when accessing submenu
                if (!isUsingSubmenu) {

                    // Alternates between menu options
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                        optionIndex = (optionIndex + 1) % NO_OF_OPTIONS;
                        HighlightOption(optionIndex);
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                        optionIndex = (optionIndex + (NO_OF_OPTIONS - 1)) % NO_OF_OPTIONS;
                        HighlightOption(optionIndex);
                    }

                    // Confirms option
                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
                        ChooseOption();
                    }
                }

                // When in quit menu, temporarily hides
                if (Script_IngameSubmenu_Settings.isPonderingQuitting ||
                    Script_IngameSubmenu_SettingsBattle.isPonderingQuitting) {
                    hideForQuitMenu = true;
                    canvasGroup.alpha = 0;
                }
            }

            // If is hiding for quit menu, shows menu back up
            if (!Script_IngameSubmenu_Settings.isPonderingQuitting &&
                !Script_IngameSubmenu_SettingsBattle.isPonderingQuitting
                && hideForQuitMenu) {
                canvasGroup.alpha = 1;
                hideForQuitMenu = false;
            }
        }
    }

    private void ChooseOption() {
        // Refreshes/closes all submenus
        foreach (var menu in submenu) {
            menu.alpha = 0;
            menu.GetComponent<Script_IngameSubmenu>().enabled = false;
        }

        submenu[optionIndex].alpha = 1;
        submenu[optionIndex].GetComponent<Script_IngameSubmenu>().enabled = true;

        isUsingSubmenu = true;

        // Deck
        if (optionIndex == 0) {
            if (menuMode == MenuMode.Overworld) {
                Script_IngameSubmenu_Deck.instance.ShowItem();
            }
            else if (menuMode == MenuMode.Battle) {
                Script_IngameSubmenu_DeckBattle.instance.ShowItem();
            }
        }

        // Inventory
        if (optionIndex == 1 && menuMode == MenuMode.Overworld) {
            Script_IngameSubmenu_Inventory.instance.ShowItem();
        }

        // Encyclopedia
        if (optionIndex == NO_OF_OPTIONS - 2) {
        }

        // Settings
        if (optionIndex == NO_OF_OPTIONS - 1) {
        }
    }

    private void HighlightOption(int index) {
        for (int i = 0; i < NO_OF_OPTIONS; i++) {
            menuOption[i].rectTransform.position = new Vector3((i == index) ? 255f : 225f, original_pos[i].y, 0f);
            menuOption[i].sprite = (i == index) ? highlightSprite[i] : baseSprite[i];
        }
    }

}
