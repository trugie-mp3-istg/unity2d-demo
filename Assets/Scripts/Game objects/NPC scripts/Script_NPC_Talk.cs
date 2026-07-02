using UnityEngine;

public class Script_NPC_Talk : MonoBehaviour
{
    private Rigidbody2D npc_rb;
    public Animator npc_anim;
    public Animator npc_prompt;
    public DialogueFlagChecker flagChecker;

    // Disables nearby NPCs' dialogues
    // when player's facing is eligible for object interaction,
    // preventing a softlock from dialogue looping
    public static bool npcDialogueActive = false;

    public Script_NPC_Walk npc_walk;

    private void Awake() {
        npc_rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        // Switches to talking mode
        npc_rb.linearVelocity = Vector2.zero;
        npc_walk.CharacterStops();
        npc_prompt.Play("npc_talk_prompt"); // Talk bubble appears
    }

    private void OnDisable() {
        // Switches back to walking mode
        npc_prompt.Play("npc_talk_no_prompt"); // Talk bubble disappears
    }

    private void Update() {
        if (DialogueManager.justEndedDialogue) return;

        // Only permits triggering dialogue when menu is down
        if (!Script_IngameMenu.isMenuActive) {
            // Talks only if there is no ongoing conversation
            // Only works if the player is not currently interacting with objects (likewise with objects)
            if (!Script_ObjectInteract.objectDialogueActive && Input.GetKeyDown(PLAYER_INPUT.interact)) {

                if (DialogueManager.isDialogueActive) {
                    DialogueManager.instance.AdvanceDialogue();
                }

                else if (flagChecker != null) {
                    var dialogue = flagChecker.GetDialogue();
                    if (dialogue != null) {

                        npcDialogueActive = true;
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