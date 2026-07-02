using System.Collections.Generic;
using UnityEngine;

public class UNUSED_Script_BulletSpellcard5_UIAttack_UNUSED : MonoBehaviour
{
    private RectTransform attackRect;
    private HashSet<Enemy_HP> alreadyHit = new HashSet<Enemy_HP>();
    void Awake() {
        attackRect = GetComponent<RectTransform>();
    }

    void CheckEnemyHits() {
        foreach (var enemy in FindObjectsByType<Enemy_HP>(FindObjectsSortMode.None)) {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            if (RectTransformUtility.RectangleContainsScreenPoint(attackRect, screenPos)) {
                if (!alreadyHit.Contains(enemy)) {
                    enemy.getDamaged(3f, ElementType.Fire);
                    alreadyHit.Add(enemy);
                }
            }
        }
    }

    void Update() {
        CheckEnemyHits();
    }
}
