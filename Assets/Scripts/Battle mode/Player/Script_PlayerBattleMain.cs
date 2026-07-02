using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

// Takes care of the actual hitbox.
public class Script_PlayerBattleMain : MonoBehaviour
{
    public float iframeCooldown = 3f;
    private float iframeAnimCooldown = 0.4f;
    public Animator player_anim;
    public string notHurtAnim;
    public string hurtAnim;

    public static bool isInv = false; // The actual invulnerability cooldown
    public static bool isHurt = false; // For tracking animation only
    public static bool isHealing = false; // For updating Script_HPBar only; Script_HPBar handles setting it to false
    private bool isInBullet = false;
    private float InvCooldown = 0f;
    private float AnimCooldown = 0f;
    private SpriteRenderer sprite;

    public static float maxHP = 4f;
    public static float curHP = -1;
    private float Damage = 0f;

    public bool godMode = false; // ======================================= For testing purposes

    void Start() {
        curHP = maxHP;
    }
    void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D bullet) {
        var b = bullet.GetComponent<Script_Bullet>();

        if (b != null && b.owner == Script_Bullet.Owner.Enemy && !Script_PlayerBattle.isDashing) {
            isInBullet = true;

            Damage = b.damage;
            if (b.colType == Script_Bullet.CollisionType.Colliding) {
                Destroy(bullet.gameObject);
            }     
        }
    }

    void FixedUpdate() {

        // ======================================= For testing purposes
        if (godMode)
            return;

        // Dashing negates damage!
        if (isInBullet && !isInv && !Script_PlayerBattle.isDashing) {
            getDamaged();
        }

        if (isInv) {
            InvCooldown -= Time.fixedDeltaTime;
            AnimCooldown -= Time.fixedDeltaTime;
            if (AnimCooldown <= 0) {
                isHurt = false;
                Color c = sprite.color;
                c.a = 0.3f;
                sprite.color = c;
                player_anim.Play(notHurtAnim);

            }
            if (InvCooldown <= 0) {
                isInv = false;
                isInBullet = false;
                Color c = sprite.color;
                c.a = 1f;
                sprite.color = c; 
            } 
        }
    }

    // When damaged: sets status isHurt to true, registers the damage, triggers i-frames, and plays hurt animation
    public void getDamaged() {
        isHurt = true;
        curHP -= Damage;
        if (curHP <= 0) {
            Die();
            return;
        }
        isInv = true;
        player_anim.Play(hurtAnim, -1, 0f);
        InvCooldown = iframeCooldown;
        AnimCooldown = iframeAnimCooldown;
    }

    // When healed:
    public void getHealed(float heal) {
        isHealing = true;
        curHP = Mathf.Min(curHP + heal, maxHP);
    }

    void Die() {
        EditorApplication.isPlaying = false;
        return;
    }
}
