using UnityEngine;

public class Script_SpellCard_4_8Directional_Active : Script_SpellCard 
{
    public override void Use() {
        foreach (var shooter in FindObjectsByType<Script_PlayerShootOnce>(FindObjectsSortMode.None)) {
            if (shooter.spellcardID == 4) {
                Script_PlayerBattle.instance.GetComponent<Script_PlayerBattle>().StartCoroutine(FireAndWait(shooter));
                return;
            }
        }
    }

    private System.Collections.IEnumerator FireAndWait(Script_PlayerShootOnce shooter) {
        shooter.Fire();
        yield return new WaitForSeconds(0.4f);
        shooter.Fire();
        yield return new WaitForSeconds(0.4f);
        shooter.Fire();
    }

    private System.Collections.IEnumerator FireAndWaitMajor(Script_PlayerShootOnce shooter) {
        shooter.colType = Script_Bullet.CollisionType.Bouncing;
        shooter.damage = 0.75f;

        shooter.Fire();
        yield return new WaitForSeconds(0.4f);
        shooter.Fire();
        yield return new WaitForSeconds(0.4f);
        shooter.Fire();

        shooter.colType = Script_Bullet.CollisionType.Colliding;
        shooter.damage = 0.5f;
    }

    public override void MajorActiveUse() {
        foreach (var shooter in FindObjectsByType<Script_PlayerShootOnce>(FindObjectsSortMode.None)) {
            if (shooter.spellcardID == 4) {
                Script_PlayerBattle.instance.GetComponent<Script_PlayerBattle>().StartCoroutine(FireAndWaitMajor(shooter));
                return;
            }
        }
    }
}
