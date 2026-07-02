using UnityEngine;

public class Script_Popup : MonoBehaviour {
    public DialogueFlagChecker flagChecker;
    private bool touchingPlayer = false;
    private void OnTriggerEnter2D(Collider2D collision) {
        // Triggers and initiates dialogue if player collides with a popup triggerer
        if (collision.gameObject.tag == "Player_interactionBox") {
            touchingPlayer = true;
            var dialogue = flagChecker.GetDialogue();
            if (dialogue != null) {

                // If the dialogue has a flag to set, sets it
                if (dialogue.flags != null && dialogue.flags.Length > 0 &&
                    !string.IsNullOrEmpty(dialogue.flags[0].setsFlag)) {
                    GameStateManager.instance.SetFlag(dialogue.flags[0].setsFlag);
                }
                DialogueManager.instance.StartDialogue(dialogue);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player_interactionBox") {
            touchingPlayer = false;
        }
    }

    void Update() {
        if (DialogueManager.justEndedDialogue)
            return;

        // Only permits triggering dialogue when menu is down
        if (!Script_IngameMenu.isMenuActive) {
            if (touchingPlayer && Input.GetKeyDown(PLAYER_INPUT.interact)) {
                if (DialogueManager.isDialogueActive && !DialogueManager.isChoosingOption) {
                    DialogueManager.instance.AdvanceDialogue();
                }
            }
        }
    }
}

