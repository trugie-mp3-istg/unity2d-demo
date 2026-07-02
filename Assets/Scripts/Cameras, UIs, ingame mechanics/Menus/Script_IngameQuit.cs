using TMPro;
using UnityEngine;

public class Script_IngameQuit : MonoBehaviour
{
    public CanvasGroup quitGameCanvasGroup;
    public TMP_Text[] quitNoYes;

    private int optionIndex = 0; // 0: No; 1: Yes

    void Start() {
        // Defaults to selecting first option first
        HighlightOption(optionIndex);
        enabled = false;

        quitGameCanvasGroup.alpha = 0;
    }

    void Update() {
        if (Script_IngameSubmenu_Settings.isPonderingQuitting ||
            Script_IngameSubmenu_SettingsBattle.isPonderingQuitting) {

            quitGameCanvasGroup.alpha = 1;

            // Alternating between menu options
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                optionIndex = (optionIndex + 1) % 2;
                HighlightOption(optionIndex);
            }

            // Confirming option
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
                if (optionIndex == 1) {
                    GENERIC_METHODS.QuitGame();
                }
                else {
                    quitGameCanvasGroup.alpha = 0;
                    enabled = false;
                    Script_IngameSubmenu_Settings.isPonderingQuitting = false;
                    Script_IngameSubmenu_SettingsBattle.isPonderingQuitting = false;
                }
            }
        }
    }

    public void HighlightOption(int index) {
        for (int i = 0; i < 2; i++) {
            var text = quitNoYes[i].GetComponentInChildren<TMP_Text>();
            text.color = (i == index) ? Color.white : Color.darkRed;
        }
    }
}
