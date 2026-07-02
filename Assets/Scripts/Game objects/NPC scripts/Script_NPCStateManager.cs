using UnityEngine;

public class Script_NPCStateManager : MonoBehaviour
{
    public enum NPC_State_Manager {Walk, Talk}
    public NPC_State_Manager cur_state = NPC_State_Manager.Walk;
    private NPC_State_Manager def_state;

    public Script_NPC_Walk walk;
    public Script_NPC_Talk talk;

    void Start() {
        // Default state is walking
        def_state = cur_state;
        SwitchState(cur_state);
    }

    // Switch state between walking and talking when fired
    public void SwitchState(NPC_State_Manager new_state) {
        cur_state = new_state;
        walk.enabled = new_state == NPC_State_Manager.Walk;
        talk.enabled = new_state == NPC_State_Manager.Talk;
    }

    void OnTriggerStay2D(Collider2D other) {
        // Triggers if player's INTERACTION BOX collides with an NPC
        if (other.CompareTag("Player_interactionBox") && !Script_ObjectInteract.objectDialogueActive) {
            SwitchState(NPC_State_Manager.Talk);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player_interactionBox")) {
            SwitchState(NPC_State_Manager.Walk);
        }
    }

    private void Update() {
        // After having finished a dialogue
        if (cur_state == NPC_State_Manager.Talk && DialogueManager.isDialogueFinished) {
            SwitchState(NPC_State_Manager.Walk);
            DialogueManager.isDialogueFinished = false; // Resets this variable
        }
    }
}
