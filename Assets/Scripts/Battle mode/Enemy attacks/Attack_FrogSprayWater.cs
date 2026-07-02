using UnityEngine;
using System.Collections;

public class Attack_FrogSprayWater : Enemy_AttackBase
{
    private Script_EnemyShootOnceSpray shooter;
    void Start() {
        shooter = GetComponent<Script_EnemyShootOnceSpray>();
    }

    public override IEnumerator Execute() {
        for (int i = 0; i < Random.Range(7,13); i++) {
            shooter.Fire();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(4f);
    }
}
