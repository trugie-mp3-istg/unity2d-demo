using System.Drawing;
using UnityEngine;

public class BulletSpawnerBase : MonoBehaviour
{
    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife;
    public float speed;
    public float size = 1f;
    public float damage;
    public Script_Bullet.CollisionType colType;
    public Script_Bullet.Owner owner;
    public Script_Bullet.Category category;

    [Header("Spawner Attributes")]
    public float firingRatePerSec;
    public int noOfBullets;

    public virtual GameObject SetBullet() {
        GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Script_Bullet>().lifetime = bulletLife;
        spawnedBullet.GetComponent<Script_Bullet>().speed = speed;
        spawnedBullet.GetComponent<Transform>().localScale *= size;
        spawnedBullet.GetComponent<Script_Bullet>().damage = damage;
        spawnedBullet.GetComponent<Script_Bullet>().colType = colType;
        return spawnedBullet;
    }
}
