using UnityEngine;

// This bullet cannot be cleared, like, at all
// Only way for it to die is either to expire, or to hit terrain, or to hit its target if it's non-piercing

public class Script_Bullet_Unbreakable : Script_Bullet {
    void OnTriggerEnter2D(Collider2D col) {

        // Touching terrains, i.e. walls; applies to ALL non-bouncing bullets
        if (col.CompareTag("Battle_terrain")) {
            if (colType != CollisionType.Bouncing) {
                Die();
                return;
            }
            else {

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
    }
}
