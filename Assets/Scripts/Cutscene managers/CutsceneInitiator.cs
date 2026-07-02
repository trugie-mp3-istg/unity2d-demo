using UnityEngine;

public class CutsceneInitiator : MonoBehaviour
{
    private CutsceneManager cutsceneManager;

    void Start() {
        cutsceneManager = GetComponent<CutsceneManager>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") cutsceneManager.PlayNextElement();
    }
}
