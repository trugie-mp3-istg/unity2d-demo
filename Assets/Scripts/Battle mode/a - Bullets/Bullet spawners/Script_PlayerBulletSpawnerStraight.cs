using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerBulletSpawnerStraight : BulletSpawnerBase {

    [Header("Spell Card Tags")]
    public int spellcardID = -1;

    [Header("Spawner-specifics - Straight")]
    public float spread = 0f;

    private float bulletDistance;
    private float timer = 0f;
    private float targetRefreshRate = 0.2f;
    private float targetRefreshTimer = 0f;
    private Transform target;

    void Start() {
        // If nearest enemy is outside this range, doesn't shoot
        // Technically, since we're comparing dist to this, and dist (for the sole purpose of comparison) is squared the actual distance,
        // bulletDistance should equal to (speed * bulletLife) ^ 2.
        // Since bulletLife = 1 and speed = 20, I multiply it by 24 for a minusculely preemptative shooting.
        bulletDistance = speed * bulletLife * 24; 
    }

    public void Fire() {
        // Refreshes target
        targetRefreshTimer -= Time.deltaTime;
        if (targetRefreshTimer <= 0f) {
            FindTarget();
            targetRefreshTimer = targetRefreshRate;
        }

        // If target exists, calculates angle to target, then fires
        if (target != null) {
            Vector2 dir = (Vector2)target.transform.position - (Vector2)transform.position;
            float angleTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angleTarget);

            timer += Time.deltaTime;
            if (timer >= 1 / firingRatePerSec) {
                if (noOfBullets == 1)
                    SpawnBullet(0f);
                else
                    for (int i = 0; i < noOfBullets; i++) {
                        float angle = (-spread / 2) + i * (spread / (noOfBullets - 1f));
                        SpawnBullet(angle);
                    }
                timer = 0;
            }
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
            if (dist < nearestDist && i != null && dist < bulletDistance) {
                nearestDist = dist;
                target = i.transform;
            }
        }
    }
}
