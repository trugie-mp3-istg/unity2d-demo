using NUnit.Framework.Interfaces;
using UnityEngine;

public class Script_ItemPickup : MonoBehaviour
{
    public Script_ItemBase itemData;
    public DialogueManager_Dialogue pickupDialogue;

    private bool touchingPlayer = false;

    // pickedUp prevents a softlock where upon picking up the item, its gameObject self-destructs, and dialogue cannot be incremented
    private bool pickedUp = false;

    void PickUpItem() {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            touchingPlayer = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
            touchingPlayer = false;
    }

    void Update() {
        if (!pickedUp) {
            if (touchingPlayer && Input.GetKeyDown(PLAYER_INPUT.interact)) {
                pickedUp = true;
                Script_PlayerInventory.instance.AddItem(itemData);

                if (pickupDialogue != null) {
                    GetComponent<Collider2D>().enabled = false;
                    if (TryGetComponent(out SpriteRenderer sr))
                        sr.enabled = false;

                    DialogueManager.instance.StartDialogue(pickupDialogue, itemData.itemName);
                }
                else {
                    Destroy(gameObject);
                }
            }
        }

        else {
            if (Input.GetKeyDown(PLAYER_INPUT.interact)) {
                if (DialogueManager.isDialogueActive && !DialogueManager.isChoosingOption) {
                    DialogueManager.instance.AdvanceDialogue();
                }
            }
            if (DialogueManager.isDialogueFinished) {
                Destroy(gameObject);
            }
        }
    }
}