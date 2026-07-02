using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private CutsceneElement_Base[] cutsceneElements;
    private int index = -1;

    void Start() {
        cutsceneElements = GetComponents<CutsceneElement_Base>();
    }

    private void ExecuteCurrentElement() {
        if (index >= 0 && index < cutsceneElements.Length) {
            cutsceneElements[index].Execute();
        }
    }

    public void PlayNextElement() {
        index++;
        ExecuteCurrentElement();
    }
}
