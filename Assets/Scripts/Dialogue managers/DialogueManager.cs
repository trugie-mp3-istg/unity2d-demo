using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Ref")]
    public CanvasGroup canvasGroup;
    public Image actorPortrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;

    public TMP_Text[] dialogueOption;

    public static bool isDialogueActive = false;

    // Forces NPCs out of talk state upon finishing dialogue,
    // else they stand there waiting to talk forever; see Script_NPCStateManager
    public static bool isDialogueFinished = false;

    // Ensures exiting dialogue doesn't retrigger it; see Script_NPC_basic_talk and Script_ObjectInteract
    public static bool justEndedDialogue = false; 

    private DialogueManager_Dialogue curDialogue;
    private int dialogueIndex;

    // Defaults to the first index option
    private int optionIndex = 0;

    public static bool isChoosingOption = false;

    /* See also:
    Script_ObjectInteract.objectDialogueActive;
    Script_NPC_basic_talk.npcDialogueActive; 
    */

    // THERE'S A FUCKING BUG WITH FLAG TRIGGER'S DIALOGUE OPTIONS
    // WHERE IT SKIPS THE CHOOSING ENTIRELY AND PREEMPTIVELY PLAYS THE FIRST OPTION'S DIALOGUES
    // IT IS INCOMPREHENSIBLE AND NOT GUARANTEED REPRODUCIBLE
    // I'M LITERALLY THROWING BULLSHIT HOPING IT KILLS ITSELF
    // apparently the bug has been that Z key gets registered once too many and accidentally prompts selection
    // but after it plays the last line of the option dialogue, you have to press an empty Z again
    // either that or Z replays the last line of the dialogue (because the bug doesn't proc with empty options)
    // (unsure since dialogue currently displays the entire line at once instead of letter by letter)
    // while that problem persists with the quick fix, at least the option choosing works relatively fine
    private bool canAcceptInput = false;

    private string contextObjectName; // For dynamic naming (e.g. items, player names, etc.)

    private void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        // Sets and resets attributes
        canvasGroup.alpha = 0;
        isDialogueActive = false;
        isDialogueFinished = false;
        justEndedDialogue = false;

        foreach (var option in dialogueOption) {
            option.gameObject.SetActive(false);
        }
    } 

    public void StartDialogue(DialogueManager_Dialogue dialogueSO, string contextName = null) {
        contextObjectName = contextName;
        curDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        isDialogueFinished = false;
        canvasGroup.alpha = 1;
        ShowDialogue();
    }

    // Advances through the dialogue
    public void AdvanceDialogue() {
        if (curDialogue == null) {
            EndDialogue();
            return;
        }
        if (dialogueIndex < curDialogue.lines.Length) ShowDialogue();
        else ShowDialogueOptions();
    }

    // Sets Dialogue UI components to the corresponding images and texts
    private void ShowDialogue() {
        DialogueLine line = curDialogue.lines[dialogueIndex];
        actorPortrait.sprite = line.actor.actorPortrait;
        actorName.text = line.actor.actorName;
        dialogueText.text = contextObjectName != null
            ? line.text.Replace("{context}", contextObjectName)
            : line.text;
        dialogueIndex++;
    }

    // Shows dialogue options at the end of a branching dialogue, if any
    private void ShowDialogueOptions()  { 
        if (curDialogue.options.Length > 0) {
            isChoosingOption = true;
            canAcceptInput = false;

            optionIndex = 0;
            for (int i = 0; i < curDialogue.options.Length; i++) {
                var option = curDialogue.options[i];
                dialogueOption[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                dialogueOption[i].gameObject.SetActive(true);
            }
            HighlightOption(optionIndex);

            StartCoroutine(EnableInputDelay());
        }
        else EndDialogue();

        IEnumerator EnableInputDelay() {
            yield return new WaitForEndOfFrame();
            canAcceptInput = true;
        }
    }

    // Swaps options' color between highlighted (yellow) and normal (white)
    private void HighlightOption(int index) {
        for (int i = 0; i < curDialogue.options.Length; i++) {
            var text = dialogueOption[i].GetComponentInChildren<TMP_Text>();
            text.color = (i == index) ? Color.yellow : Color.white;
        }
    }

    void Update() {
        if (!isChoosingOption) return;

        // Alternating between dialogue options
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            optionIndex = (optionIndex + 1) % curDialogue.options.Length;
            HighlightOption(optionIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            optionIndex = (optionIndex - 1 + curDialogue.options.Length) % curDialogue.options.Length;
            HighlightOption(optionIndex);
        }

        // Confirming option
        if (canAcceptInput && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))) {
            ChooseOption();
        }
    }

    private void ChooseOption() {
        isChoosingOption = false;
        foreach (var option in dialogueOption) { 
            option.gameObject.SetActive(false); 
        }
        var chosen_dialogue = curDialogue.options[optionIndex].nextDialogue;
        if (chosen_dialogue != null) {
            StartDialogue(chosen_dialogue);
            dialogueIndex = 0;

            // Sets flag
            if (chosen_dialogue.flags != null && chosen_dialogue.flags.Length > 0 &&
                !string.IsNullOrEmpty(chosen_dialogue.flags[0].setsFlag)) {
                GameStateManager.instance.SetFlag(chosen_dialogue.flags[0].setsFlag);
            }
        }
        else EndDialogue();
    }

    void LateUpdate() {
        if (justEndedDialogue)
            justEndedDialogue = false;
    }

    private void EndDialogue() {
        // Resets attributes
        dialogueIndex = 0;
        isDialogueActive = false;
        Script_NPC_Talk.npcDialogueActive = false;
        Script_ObjectInteract.objectDialogueActive = false;
        justEndedDialogue = true;
        isDialogueFinished = true;
        canvasGroup.alpha = 0;
    }
}
