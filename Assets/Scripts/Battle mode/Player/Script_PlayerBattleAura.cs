using UnityEngine;

public class Script_PlayerBattleAura : MonoBehaviour
{
    public Animator player_anim;
    public string notHurtAnim;
    public string hurtAnim;

    private int isInBullet = 0; // Essentially a bool, except it accounts for when player is inside multiple bullets
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color c = sprite.color;
        c.a = 0.05f;
        sprite.color = c;
    }

    void OnTriggerEnter2D(Collider2D bullet) {
        var b = bullet.GetComponent<Script_Bullet>();
        if (b != null && b.owner == Script_Bullet.Owner.Enemy) isInBullet += 1;
    }
    

    void OnTriggerExit2D(Collider2D bullet) {
        var b = bullet.GetComponent<Script_Bullet>();
        if (b != null && b.owner == Script_Bullet.Owner.Enemy) isInBullet -= 1;
    }

    void FixedUpdate() {
        Color c = sprite.color;

        if (Script_PlayerBattleMain.isHurt) {
            c.a = 0.75f;
            player_anim.Play(hurtAnim);
        }
        else {
            player_anim.Play(notHurtAnim);
            if (isInBullet > 0) {
                c.a = 0.3f;
            }
            else c.a = 0.05f;
        }  

        sprite.color = c;
    }

    void Update() {
        // Visual effect
        transform.rotation = Quaternion.Euler(0f, 0f, -Time.time * 90);
    }
}

