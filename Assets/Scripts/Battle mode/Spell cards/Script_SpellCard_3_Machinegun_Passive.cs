using UnityEngine;

public class Script_SpellCard_3_Machinegun_Passive : Script_SpellCard
{
    private bool passiveActive = false;
    
    public override void Use() {
        passiveActive = true;
    }

    void Update() {
        if (passiveActive) {
            foreach (var shooter in FindObjectsByType<Script_PlayerBulletSpawnerStraight>(FindObjectsSortMode.None)) {
                if (shooter.spellcardID == 3) {
                    shooter.Fire();
                    return;
                }
            }
        }
    }
}
