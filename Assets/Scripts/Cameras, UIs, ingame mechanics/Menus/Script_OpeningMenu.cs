using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_OpeningMenu : MonoBehaviour
{
    public string nextScene;
    public TMP_Text[] menuOption;

    // Defaults to the first index option
    private int optionIndex = 0;
    private int NO_OF_OPTIONS = 4; // New Game, Continue, Settings, Quit

    void Start() {
        // Defaults to selecting first option first
        HighlightOption(optionIndex);
    }

    void Update() {
        // Alternating between menu options
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

    private void ChooseOption() {
        // New Game
        if (optionIndex == 0) {
            if (nextScene != null) SceneManager.LoadScene(nextScene);
        }

        // Continue
        if (optionIndex == 1) {
            if (nextScene != null)
                SceneManager.LoadScene(nextScene);
        }

        // Settings
        if (optionIndex == 2) {
        }

        // Quit; always the last option
        if (optionIndex == NO_OF_OPTIONS - 1) {
            GENERIC_METHODS.QuitGame();
        }
    }

    // Swaps options' color between highlighted (yellow) and normal (white)
    private void HighlightOption(int index) {
        for (int i = 0; i < NO_OF_OPTIONS; i++) {
            var text = menuOption[i].GetComponentInChildren<TMP_Text>();
            text.color = (i == index) ? Color.yellow : Color.white;
        }
    }
}
