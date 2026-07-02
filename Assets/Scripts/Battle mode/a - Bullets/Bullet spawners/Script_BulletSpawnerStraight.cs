using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawnerStraight : BulletSpawnerBase {

    [Header("Spawner-specifics - Straight")]
    public float spread = 0f;

    private float timer = 0f;
    private float outOfRangeDistance;

    private Script_PlayerBattleMain player;

    void Start() {
        player = FindFirstObjectByType<Script_PlayerBattleMain>();
        outOfRangeDistance = GetComponentInParent<EnemyAI_Ranged_Simple>().outOfRangeDistance;
    }

    void Update() {
        // Calculates angle to player because LookAt() won't fucking work for 2D
        Vector2 dir = (Vector2)player.transform.position - (Vector2)transform.position;
        float distance_to_player = dir.magnitude;
        float anglePlayer = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, anglePlayer);

        if (distance_to_player >= outOfRangeDistance) {
            timer = 0f; // Resets so no burst fire on re-entry
            return;
        }

        timer += Time.deltaTime;
        if (timer >= 1 / firingRatePerSec) {
            if (noOfBullets == 1)
                SpawnBullet(0f);
            else for (int i = 0; i < noOfBullets; i++) {
                float angle = (-spread / 2) + i * (spread / (noOfBullets - 1f));
                SpawnBullet(angle);
            }
            timer = 0;
        }
    }

    private void SpawnBullet(float angle) {
        if (bullet) {
            GameObject spawnedBullet = base.SetBullet();
            spawnedBullet.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        }
    }
}
