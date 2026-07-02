// Lunge movement only

using UnityEngine;

public class Script_EnemyLunge : MonoBehaviour {

    [Header("Lunge Attributes")]
    public float lungeSpeed = 6f;
    public float lungeDuration = 0.3f;

    public Rigidbody2D enemy_rb;

    private Vector2 lungeDirection;
    private bool isLunging = false;
    private bool hitTerrain = false;
    public bool IsLunging => isLunging;

    // Gets player's position and lunges itself in that direction (dumbass)
    public void Lunge(Vector2 target_pos) {
        lungeDirection = (target_pos - (Vector2)transform.position).normalized;
        isLunging = true;
        hitTerrain = false;
    }

    void FixedUpdate() {
        if (!isLunging)
            return;
        if (hitTerrain) {
            EndLunge();
            return;
        }

        enemy_rb.linearVelocity = lungeDirection * lungeSpeed;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!isLunging)
            return;
        if (col.CompareTag("Battle_terrain")) {
            hitTerrain = true;
        }
    }

    public void EndLunge() {
        isLunging = false;
        enemy_rb.linearVelocity = Vector2.zero;
    }
}