using UnityEngine;

public class Script_BulletSpellcard1_EndExplosion : MonoBehaviour
{
    float damage = 1f;
    void Start() {
        float radius = GetComponent<SpriteRenderer>().bounds.extents.x;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var enemy in enemies) {
            // Here it specifically overlays the Enemy_HPDetection script to call the Enemy_HP script in the parent gameObject,
            // because there's only one collider that has Enemy_HPDetection with it in the same child gameObject.
            // If this function is to indiscriminately overlay the parent gameObject's Enemy_HP,
            // it triggers 2 colliders, one in the parent gameObject and one in the child Sprite gameObject,
            // thus taking that same damage twice.
            Enemy_HPDetection det = enemy.GetComponent<Enemy_HPDetection>();
            if (det != null)
                det.GetComponentInParent<Enemy_HP>().getDamaged(damage, element: ElementType.Water);
        }
    }
}
