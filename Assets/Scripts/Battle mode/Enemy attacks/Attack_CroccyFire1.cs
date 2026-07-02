using UnityEngine;
using System.Collections;

public class Attack_CroccyFire1 : Enemy_AttackBase
{
    private Script_EnemyShootOnce shooter;
    void Start() {
        shooter = GetComponent<Script_EnemyShootOnce>();
    }

    public override IEnumerator Execute() {
        for (int i = 0; i < 2; i++) {
            shooter.Fire();
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < 3; i++) {
            shooter.Fire();
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(2.5f);
    }
}
