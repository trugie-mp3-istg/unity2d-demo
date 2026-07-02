using System.Collections;
using UnityEngine;

public class CutsceneElement_Base : MonoBehaviour
{
    public float duration;
    private CutsceneManager cutsceneManager;

    void Start() {
        cutsceneManager = GetComponent<CutsceneManager>();
    }

    public virtual void Execute() {
    }

    protected IEnumerator WaitAndAdvance() {
        yield return new WaitForSeconds(duration);
        cutsceneManager.PlayNextElement();
    }
}
