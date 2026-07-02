using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BattleManager : MonoBehaviour
{
    public static BattleManager bm;
    public string overworldScene;
    
    void Awake() {
        bm = this;
    }

    public virtual void OnKill() {
    }

    public virtual void OnItemPickup() {
    }

    protected abstract void CheckWinCondition();

    protected virtual void OnBattleEnd() {
        bm = null;
        // Functionally the if-condition does nothing;
        // it only ensures the engine doesn't return an error when testing directly through the battle scene
        if (Script_Player.instance != null) 
            Script_Player.instance.gameObject.SetActive(true);
        SceneManager.LoadScene(overworldScene);
    }
}
