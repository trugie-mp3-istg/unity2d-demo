using UnityEngine;

// The explosion script handles dealing 1 explosion damage, not this script or the spawner prefab
public class Script_SpellCard_1_FantasySeal_Active : Script_SpellCard
{
    public override void Use() {
        foreach (var shooter in FindObjectsByType<Script_PlayerShootOnce>(FindObjectsSortMode.None)) {
            if (shooter.spellcardID == 1) {
                shooter.Fire();
                return;
            }
        }
    }

    public override void MajorActiveUse() {
        foreach (var shooter in FindObjectsByType<Script_PlayerShootOnce>(FindObjectsSortMode.None)) {
            if (shooter.spellcardID == 1) {
                shooter.speed -= 3f;
                shooter.bulletLife += 2f;
                shooter.size = 1.5f;

                shooter.Fire();

                shooter.speed += 3f;
                shooter.bulletLife -= 2f;
                shooter.size = 1f;
                return;
            }
        }
    }
}
