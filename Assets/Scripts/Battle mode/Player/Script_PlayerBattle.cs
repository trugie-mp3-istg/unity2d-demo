using System.Threading;
using UnityEngine;

public class Script_PlayerBattle : MonoBehaviour
{
    public Rigidbody2D player_rb;
    public float speed = 8f;

    // Dashing presets
    public float dashSpeed = 3.2f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    // Timer that runs until reaching presets
    private float Duration = 0f;
    private float Cooldown = 0f;
    public static bool isDashing = false;

    public static GameObject instance;
    void Awake() {
        instance = gameObject;
    }

    void Update() {
        if ((Input.GetKeyDown(PLAYER_INPUT.dash))
            && Cooldown <= 0) {
            isDashing = true;
            Duration = dashDuration;
            Cooldown = dashCooldown;
        }
    }

    void FixedUpdate() {

        Duration -= Time.deltaTime;
        Cooldown -= Time.deltaTime;

        if (!DialogueManager.isDialogueActive) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Movement
            float isCrouching = 1f;
            if (Input.GetKey(PLAYER_INPUT.crouchfocus))
                isCrouching = 0.5f;
            if (isDashing) {
                // If player is standing still, defaults to dashing forward. Else, dash normally
                // Dashing is unaffecting by crouching, i.e. player still dashes at full speed
                if (horizontal == 0 && vertical == 0) {
                    player_rb.linearVelocity = new Vector2(1, 0) * speed * dashSpeed;
                }
                else player_rb.linearVelocity = new Vector2(horizontal, vertical) * speed * dashSpeed;
                if (Duration <= 0) isDashing = false;
            }
            else player_rb.linearVelocity = new Vector2(horizontal, vertical) * speed * isCrouching;
        }

        // If talking then no walking
        else {
            // Halts movement
            player_rb.linearVelocity = Vector2.zero;
        }
    }
}
