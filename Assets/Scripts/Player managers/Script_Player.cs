using UnityEngine;

public class Script_Player : MonoBehaviour
{
    public Rigidbody2D player_rb;
    public Animator player_anim;
    public float speed = 4.5f;
    public int facingDirection = 0; // 0 = Down, 1 = Right, 2 = Up, 3 = Left

    public Transform interactionBox;
    public float interactionDistance = 1f;

    private readonly Vector2[] direction = {
        Vector2.down,
        Vector2.right,
        Vector2.up,
        Vector2.left
    };

    public static Script_Player instance;
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void FixedUpdate() {
        if (!DialogueManager.isDialogueActive) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Sets movement sprites
            player_anim.SetFloat("horizontal", horizontal);
            player_anim.SetFloat("vertical", vertical);

            // Updates direction when moving for interaction box
            if (horizontal > 0)
                facingDirection = 1;
            else if (horizontal < 0)
                facingDirection = 3;
            else if (vertical > 0)
                facingDirection = 2;
            else if (vertical < 0)
                facingDirection = 0;

            // Shift to run at (approx.) double speed
            float if_running = 1f;
            player_anim.SetFloat("run", 1f);
            if (Input.GetKey(PLAYER_INPUT.run)) {
                if_running = 1.8f;
                player_anim.SetFloat("run", 1.5f);
            }

            // Movement
            player_rb.linearVelocity = new Vector2(horizontal, vertical) * speed * if_running;

            // Interaction box follows ahead
            interactionBox.localPosition = direction[facingDirection] * interactionDistance;
        }

        // If talking then no walking
        else {
            // Halts movement
            player_rb.linearVelocity = Vector2.zero;

            // Halts animation
            player_anim.SetFloat("horizontal", 0f);
            player_anim.SetFloat("vertical", 0f);
        }
    }
}
