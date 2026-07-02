using UnityEngine;

public class CSE_Test : CutsceneElement_Base
{
    public override void Execute() {
        StartCoroutine(WaitAndAdvance());
        Debug.Log("Executing " + name);
    }
}
