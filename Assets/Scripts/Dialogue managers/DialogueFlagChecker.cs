using UnityEngine;

// In charge of checking flags/conditions needed to play dialogue, i.e. checks needsFlag.
// As for setting flags, it is done directly in talk scripts: Script_NPC_basic_talk; Script_ObjectInteract; Script_Popup.

public class DialogueFlagChecker : MonoBehaviour {
    public DialogueManager_Dialogue[] dialogueSO;

    // With all dialogues manually ordered by ascending specific-ness,
    // increments the index until reaching a dialogue that doesn't have all flags satisfied,
    // then returns the last dialogue that does
    public DialogueManager_Dialogue GetDialogue() {
        DialogueManager_Dialogue dialogueToPlay = null;

        foreach (var dialogue in dialogueSO) {

            // If conditional hasn't been met, breaks
            if (dialogue.flags == null || dialogue.flags.Length == 0) {
                dialogueToPlay = dialogue;
                continue;
            }
            var flag = dialogue.flags[0];
            bool gate = true;

            foreach (var i in flag.needsFlag) {
                if (!GameStateManager.instance.HasFlag(i)) {
                    gate = false;
                    break;
                }
            }
            if (gate) dialogueToPlay = dialogue;
            else break;
        }
        return dialogueToPlay;
    }
}