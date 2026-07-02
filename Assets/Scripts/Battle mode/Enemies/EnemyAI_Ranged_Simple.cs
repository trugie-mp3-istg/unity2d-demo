using UnityEngine;

public class EnemyAI_Ranged_Simple : MonoBehaviour
{
    public enum modeList { Brash, Coward }
    public modeList mode = modeList.Brash;

    public Rigidbody2D enemy_rb;

    public float triggerDistance = 0f;
    public float outOfRangeDistance = 1f;
    public float speed = 1f;
    public float moveTime = 1f;
    public float cooldown = 1f; // Cooldown before moving again
    private float mt = 0f;
    private float cd = 0f;
    private bool isMoving = false;

    private Script_PlayerBattleMain player;

    void Start()
    {
        player = FindFirstObjectByType<Script_PlayerBattleMain>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mt -= Time.fixedDeltaTime;
        cd -= Time.fixedDeltaTime;

        float distance_to_player = Vector2.Distance(player.transform.position, gameObject.transform.position);

        if (distance_to_player < outOfRangeDistance) {
        
        // Mode "Brash": enemy will approach player if far enough (and not too far) away (with cooldown),
        // stopping at triggerDistance away from player.
        if (mode == modeList.Brash) {
            if (distance_to_player > triggerDistance) {
                if (cd <= 0 && !isMoving) {
                    isMoving = true;
                    mt = moveTime;
                }

                if (isMoving) {
                    Move(1);
                    if (mt <= 0) {
                        isMoving = false;
                        cd = cooldown;
                        enemy_rb.linearVelocity = Vector2.zero;
                    }
                }
            }
            else {
                isMoving = false;
                cd = cooldown;
                enemy_rb.linearVelocity = Vector2.zero;
            }
        }

            // Mode "Coward": enemy will back off from player (with cooldown) if player is within triggerDistance.
            if (mode == modeList.Coward) {
                if (distance_to_player < triggerDistance) {
                    if (cd <= 0 && !isMoving) {
                        isMoving = true;
                        mt = moveTime;
                    }
                }

                // Safeguards in case enemy moves itself out of triggerDistance, refreshing cooldown, thus moving continuously
                else if (cd > 0 && mt <= 0) {
                    isMoving = false;
                    enemy_rb.linearVelocity = Vector2.zero;
                }

                if (isMoving) {
                    Move(-1);
                    if (mt <= 0) {
                        isMoving = false;
                        cd = cooldown;
                        enemy_rb.linearVelocity = Vector2.zero;
                    }
                }
            }
        }
    }

    // Grid-based-like movement
    // Movement functions prioritize horizontal movement over vertical

    void Move(float mode) {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) >=
            Mathf.Abs(player.transform.position.y - transform.position.y)) {
            float x = player.transform.position.x < transform.position.x ? -1 : 1;
            enemy_rb.linearVelocity = new Vector2(x, 0) * speed * mode;
        }
        else {
            float y = player.transform.position.y < transform.position.y ? -1 : 1;
            enemy_rb.linearVelocity = new Vector2(0, y) * speed * mode;
        }
    }
}


