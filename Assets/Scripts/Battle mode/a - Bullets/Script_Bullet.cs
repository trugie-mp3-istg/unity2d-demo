using UnityEngine;

public class Script_Bullet : MonoBehaviour {
    public enum Owner {
        Player, Enemy
    }
    public enum Category {
        Normal, Clear, Explosion
    }
    public enum CollisionType {
        Colliding, Piercing, Bouncing
    };

    public Owner owner;
    public Category category = Category.Normal;
    public CollisionType colType;
    public ElementType element;
    private ElementType strongerElement;

    public float lifetime = 1f;
    public float rotation = 0f;
    public float speed = 1f;
    public float damage = 1f;

    private Vector2 bounceDir;
    private Vector2 spawnpoint;
    private float timer = 0f;
    public GameObject deathAnimObject;

    void Awake() {
        // Sets strongerElement and weakerElement of element
        strongerElement = ElementTypeClass.getStrongerElement(element);
    }

    void Start() {
        spawnpoint = transform.position;
        bounceDir = transform.right;
    }

    void Update() {
        if (timer > lifetime) {
            // Spawns bullet destroy animation
            if (deathAnimObject != null) {
                Instantiate(deathAnimObject, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private Vector2 Movement(float timer) {
        float x = timer * speed * bounceDir.x;
        float y = timer * speed * bounceDir.y;
        return new Vector2(x + spawnpoint.x, y + spawnpoint.y);
    }

    void OnTriggerEnter2D(Collider2D col) {

        // Touching terrains, i.e. walls; applies to ALL non-bouncing bullets
        if (col.CompareTag("Battle_terrain")) {
            if (colType != CollisionType.Bouncing) {
                Die();
                return;
            }
            else {
                bool canBounce = true; // Bounces once
                Bounds b = col.bounds;
                Vector2 pos = transform.position;

                float distLeft = Mathf.Abs(pos.x - b.min.x);
                float distRight = Mathf.Abs(pos.x - b.max.x);
                float distBottom = Mathf.Abs(pos.y - b.min.y);
                float distTop = Mathf.Abs(pos.y - b.max.y);

                float minDist = Mathf.Min(distLeft, distRight, distBottom, distTop);

                if (minDist == distLeft || minDist == distRight) {
                    bounceDir.x *= -1;
                    canBounce = false;
                }
                else {
                    bounceDir.y *= -1;
                    canBounce = false;
                }

                // Resets spawnpoint and timer
                spawnpoint = transform.position;
                timer = 0f;

                if (!canBounce)
                    colType = CollisionType.Colliding;
            }
        }

        // Touching player; applies to non-player, non-piercing bullets
        if (col.CompareTag("Player") && owner == Owner.Enemy && colType != CollisionType.Piercing) {
            Die(against_player: true); // Script_PlayerBattleMain has its own destruction call
            return;
        }

        // Touching enemies; applies to player, non-piercing bullets
        if (col.CompareTag("Battle_Enemy") && owner == Owner.Player && colType != CollisionType.Piercing) {
            Die();
            return;
        }

        // Touching an explosion; applies to ALL non-unbreakable bullets
        if (col.CompareTag("Battle_bulletExplode")) {
            Die();
        }

        // With general collision cases out of the way, we move on to specific bullet-to-bullet interaction cases
        var other_bullet = col.GetComponent<Script_Bullet>();
        if (other_bullet != null && other_bullet.owner != owner) {
            ResolveBulletCollision(other_bullet);
        }

        var other_beam = col.GetComponent<Script_BeamUI>();
        if (other_beam != null) {
            ResolveBeamCollision(other_beam);
        }
    }

    void ResolveBulletCollision(Script_Bullet other) {

        // Clear/explosion bullets destroy all other bullets of the opposite faction

        // Clear bullets cannot clear target bullets of its greater element OR the same element
        if (other.category == Category.Clear) {
            if ((owner == Owner.Player && other.owner == Owner.Enemy) ||
                (owner == Owner.Enemy && other.owner == Owner.Player)) {
                if (element != other.element && element != other.strongerElement)
                    Die();
            }
        }

        if (other.category == Category.Explosion) {
            if ((owner == Owner.Player && other.owner == Owner.Enemy) ||
                (owner == Owner.Enemy && other.owner == Owner.Player))
                Die();
        }
    }

    void ResolveBeamCollision(Script_BeamUI other) {
        if (other.category == Script_BeamUI.Category.Clear) {
            if ((owner == Owner.Player && other.owner == Script_BeamUI.Owner.Enemy) ||
                (owner == Owner.Enemy && other.owner == Script_BeamUI.Owner.Player)) {
                if (element != other.element && element != other.strongerElement)
                    Die();
            }
        }
    }

    public void Die(bool against_player = false) {
        if (deathAnimObject != null)
            Instantiate(deathAnimObject, transform.position, transform.rotation);
        
        // Since bullet destruction upon colliding player is handled separately over Script_PlayerBattleMain
        if (!against_player)
            Destroy(gameObject);
    }
}
