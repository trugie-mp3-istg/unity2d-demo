using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_PlayerShootOnce : BulletSpawnerBase {

    [Header("Spell Card Tags")]
    public int spellcardID = -1;

    [Header("Spawner-specifics - Straight")]
    public float spread = 0f;

    private Transform target;

    public void Fire() {
        // Refreshes target
        FindTarget();

        // If target exists, calculates angle to target, then fires
        if (target != null) {
            Vector2 dir = (Vector2)target.transform.position - (Vector2)transform.position;
            float angleTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleTarget);
        }
        else
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        if (noOfBullets == 1)
            SpawnBullet(0f);
        else for (int i = 0; i < noOfBullets; i++) {
            float angle;
            if (spread != 360f) {
                angle = (-spread / 2) + i * (spread / (noOfBullets - 1f));
            }
            else
                angle = (-spread / 2) + i * (spread / noOfBullets);
            SpawnBullet(angle);
        }
    }

    private void SpawnBullet(float angle) {
        if (bullet) {
            GameObject spawnedBullet = base.SetBullet();
            spawnedBullet.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        }
    }

    // Find the nearest target. With Update(), it constantly refreshes (not on every frame)
    private void FindTarget() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Battle_Enemy");
        float nearestDist = Mathf.Infinity;
        target = null;

        foreach (var i in targets) {
            float dist = Vector2.SqrMagnitude(i.transform.position - transform.position);
            if (dist < nearestDist && i != null) {
                nearestDist = dist;
                target = i.transform;
            }
        }
    }
}
