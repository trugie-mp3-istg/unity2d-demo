// Unlike Enemy_HP which is attached on the master game object, this script is attached to the sprite game object.

using UnityEngine;

public class Enemy_HPDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        var bullet = other.GetComponent<Script_Bullet>();
        if (bullet != null && bullet.owner == Script_Bullet.Owner.Player) {
            GetComponentInParent<Enemy_HP>().getDamaged(bullet.damage, bullet.element);
        }

        var beam = other.GetComponent<Script_BeamUI>();
        if (beam != null && beam.owner == Script_BeamUI.Owner.Player) {
            GetComponentInParent<Enemy_HP>().getDamaged(beam.damage, beam.element);
        }
    }
}
