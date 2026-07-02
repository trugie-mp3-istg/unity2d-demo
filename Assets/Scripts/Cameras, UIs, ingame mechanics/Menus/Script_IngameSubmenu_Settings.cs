// To be updated later after gameplay is close to being finalized

using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class Script_IngameSubmenu_Settings : Script_IngameSubmenu
{
    public string menuScene;
    public TMP_Text[] menuOption;
    public CanvasGroup quitGame;
    public static bool isPonderingQuitting = false;

    // Defaults to the first index option
    private int optionIndex = 0;
    private int NO_OF_OPTIONS = 4; // Volume, Controls, Menu, Quit

    void Start() {
        // Defaults to selecting first option first
        HighlightOption(optionIndex);
        enabled = false;
    }

    void Update() {
        // Alternating between menu options
        if (!isPonderingQuitting) {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                optionIndex = (optionIndex + 1) % NO_OF_OPTIONS;
                HighlightOption(optionIndex);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                optionIndex = (optionIndex + (NO_OF_OPTIONS - 1)) % NO_OF_OPTIONS;
                HighlightOption(optionIndex);
            }

            // Confirming option
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
                ChooseOption();
            }
        }
    }

    public override void ChooseOption() {
        // Menu
        if (optionIndex == 2) {
            // Resets timeScale, or else the game is stuck frozen upon re-entering
            //Time.timeScale = 1f;
            //Script_IngameMenu.isMenuActive = false;
            //Script_IngameMenu.isUsingSubmenu = false;

            //SceneManager.LoadScene(menuScene);
        }

        // Quit
        if (optionIndex == NO_OF_OPTIONS - 1) {
            isPonderingQuitting = true;
            quitGame.GetComponent<Script_IngameQuit>().enabled = true;
        }
    }

    public override void HighlightOption(int index) {
        for (int i = 0; i < NO_OF_OPTIONS; i++) {
            var text = menuOption[i].GetComponentInChildren<TMP_Text>();
            text.color = (i == index) ? Color.yellow : Color.white;
        }
    }
}
