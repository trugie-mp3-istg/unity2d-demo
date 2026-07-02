using UnityEngine;

[CreateAssetMenu(fileName = "DialogueManager_Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueManager_Dialogue : ScriptableObject {
    public DialogueFlag[] flags;
    public DialogueLine[] lines;
    public DialogueOption[] options;
}

[System.Serializable] // Accessible from the inspector
public class DialogueFlag {
    public string[] needsFlag;
    public string setsFlag;
}

[System.Serializable]
public class DialogueLine {
    public DialogueManager_Actor actor;
    public string text;
}

// Used in branching dialogues; "optionText" is what the trigger to "nextDialogue" is shown as
// e.g. Question? -> choice "A"/"B", "A" -> ResponseA, "B" -> ResponseB.
// "A" and "B" are optionText; ResponseA and ResponseB are nextDialogue
[System.Serializable]
public class DialogueOption {
    public string optionText;
    public DialogueManager_Dialogue nextDialogue; 
}