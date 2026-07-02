using UnityEngine;
using UnityEngine.SceneManagement;

// GameObjects with this script transition the player (not the Player gameObject) to a battle scene after dialogue.

public class Script_SceneChanger_DialogueToBattle : MonoBehaviour {
    public string needsFlag;
    public string battleScene;
    private bool transitioning = false;

    void Update() {
        if (transitioning)
            return;
        if (GameStateManager.instance.HasFlag(needsFlag) && !DialogueManager.isDialogueActive) {
            transitioning = true;
            Script_Player.instance.gameObject.SetActive(false);

            // Turns off dialogue
            DialogueManager.isDialogueActive = false;
            DialogueManager.justEndedDialogue = false;
            // Removes flag to not send player into infinite battle loop
            GameStateManager.instance.RemoveFlag(needsFlag);

            SceneManager.LoadScene(battleScene);
        }
    }
}