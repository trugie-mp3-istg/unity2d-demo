using UnityEngine;

// GameObjects with this script disable themselves when Flag is fired and Dialogue is finished

public class DestroySelf : MonoBehaviour {
    public string needsFlag;

    void Update() {
        if (GameStateManager.instance.HasFlag(needsFlag) && !DialogueManager.isDialogueActive) {
            gameObject.SetActive(false);
        }
    }
}