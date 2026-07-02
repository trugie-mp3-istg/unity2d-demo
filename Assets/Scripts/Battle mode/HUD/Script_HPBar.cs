using UnityEngine;

public class Script_HPBar : MonoBehaviour
{
    private float maxWidth;
    private bool playerHP_changes = false;

    void Start() {
        maxWidth = GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        if (Script_PlayerBattleMain.isHurt || 
            Script_PlayerBattleMain.isHealing) {
            playerHP_changes = true;
        }
        if (playerHP_changes) {
            RefreshHPBar();
            playerHP_changes = false;
            
            if (Script_PlayerBattleMain.isHealing) Script_PlayerBattleMain.isHealing = false;
        }
    }

    // For future proofing: I choose not to call this function directly from player's HP-changing functions because:
    // 1) Bool gates work fine
    // 2) PlayerBattleMain should solely be about game logic. UI goes to where it belongs
    void RefreshHPBar() {
        float newWidth = maxWidth / Script_PlayerBattleMain.maxHP * Script_PlayerBattleMain.curHP;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
}
