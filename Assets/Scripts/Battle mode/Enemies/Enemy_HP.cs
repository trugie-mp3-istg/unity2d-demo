using UnityEngine;

public class Enemy_HP : MonoBehaviour
{
    public float HP;
    public ElementType element;
    private float maxHP;
    private Script_EnemyHPBar enemy_hpbar;

    void Start() {
        maxHP = HP;
        enemy_hpbar = GetComponentInChildren<Script_EnemyHPBar>();
    }

    void Update()
    {
        if (HP <= 0)
            Die();
    }

    public void getDamaged(float damage, ElementType element) {
        // Calculates damage from elemental interaction
        ElementType strongerElement = ElementTypeClass.getStrongerElement(element);
        ElementType weakerElement = ElementTypeClass.getWeakerElement(element);

        if (this.element == strongerElement)
            damage *= 0.5f;
        else if (this.element == weakerElement)
            damage *= 1.5f;

        // Deals damage
        HP -= damage;
        enemy_hpbar.RefreshHP(HP, maxHP);
    }

    void Die() {
        BattleManager.bm?.OnKill();
        Destroy(gameObject);
    }
}
