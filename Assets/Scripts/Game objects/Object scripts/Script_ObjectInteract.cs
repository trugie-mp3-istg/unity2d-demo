using UnityEngine;

public class Script_ObjectInteract : MonoBehaviour
{
    public DialogueFlagChecker flagChecker;

    // Disables nearby NPCs' dialogue prompting
    // when object interaction is currently playing
    public static bool objectDialogueActive = false;
    private bool playerFacingThis = false;

    void OnTriggerStay2D(Collider2D other) {
        // Player's INTERACTION BOX colliding with, i.e. facing this object
        if (other.CompareTag("Player_interactionBox")) playerFacingThis = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        // Player's INTERACTION BOX no longer colliding with, i.e. no longer facing this object
        if (other.CompareTag("Player_interactionBox")) playerFacingThis = false;
    }

    void Update() {
        if (DialogueManager.justEndedDialogue) return;

        // Only permits triggering dialogue when menu is down
        if (!Script_IngameMenu.isMenuActive) {

            // Talks only if there is no ongoing conversation
            // Only works if the player is not currently interacting with NPCs (likewise with NPCs)
            if (playerFacingThis && !Script_NPC_Talk.npcDialogueActive &&
                Input.GetKeyDown(PLAYER_INPUT.interact)) {

                if (DialogueManager.isDialogueActive && !DialogueManager.isChoosingOption) {
                    DialogueManager.instance.AdvanceDialogue();
                }

                else if (flagChecker != null) {
                    var dialogue = flagChecker.GetDialogue();
                    if (dialogue != null) {

                        objectDialogueActive = true;
                        // If the dialogue has a flag to set, sets it
                        if (dialogue.flags != null && dialogue.flags.Length > 0 &&
                            !string.IsNullOrEmpty(dialogue.flags[0].setsFlag)) {
                            GameStateManager.instance.SetFlag(dialogue.flags[0].setsFlag);
                        }
                        DialogueManager.instance.StartDialogue(dialogue);
                    }
                }
            }
        }
    }
}
