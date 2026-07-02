using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerSpin : BulletSpawnerBase {

    [Header("Spawner-specifics - Spin")]
    public float firingRotation = 1f;

    private float timer = 0f;
    private float outOfRangeDistance;

    private Script_PlayerBattleMain player;

    void Start() {
        player = FindFirstObjectByType<Script_PlayerBattleMain>();
        outOfRangeDistance = GetComponentInParent<EnemyAI_Ranged_Simple>().outOfRangeDistance;
    }

    void Update() {
        // Adapted from Script_BulletSpawnerStraight for OORD calculation
        Vector2 dir = (Vector2)player.transform.position - (Vector2)transform.position;
        float distance_to_player = dir.magnitude;

        if (distance_to_player >= outOfRangeDistance) {
            timer = 0f; // Resets so no burst fire on re-entry
            return;
        }

        timer += Time.deltaTime;
        // (Transform.eulerAngles.z + N) = z-vector = rotation speed
        // The larger N is, the faster it rotates
        // Not sure what x-vector and y-vector do?
        transform.eulerAngles = new Vector3(0f, 0f, Time.time * firingRotation); 

        if (timer >= 1/firingRatePerSec) {
            for (int i = 0; i < noOfBullets; i++) {
                float angle = i * (360f / noOfBullets);
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
