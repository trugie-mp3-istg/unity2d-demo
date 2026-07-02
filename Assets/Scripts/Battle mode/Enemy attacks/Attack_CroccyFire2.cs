using UnityEngine;
using System.Collections;

public class Attack_CroccyFire2 : Enemy_AttackBase
{
    private Script_EnemyShootOnce shooter;
    void Start() {
        shooter = GetComponent<Script_EnemyShootOnce>();
    }

    public override IEnumerator Execute() {
        for (int i = 0; i < 2; i++) {
            shooter.Fire();
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(2.5f);
    }
}
