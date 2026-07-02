using UnityEngine;

public class Script_EnemyHPBar : MonoBehaviour
{
    public SpriteRenderer sprite;

    public Transform fill;
    private float fill_bar;

    void Start()
    {
        // Calculates enemies' sprite's height, then sets the HP bar floating above that
        float pos = -1;
        if (sprite != null) {
            pos = sprite.bounds.extents.y;
            transform.localPosition = new Vector3(0f, pos + 2f, 0f);
        }

        // Sets fill_bar to be the HP bar's filling's y-scaling
        // Idea is to have the filling chipped off as the enemy takes damage, until it is emptied (HP = 0)
        fill_bar = fill.localScale.y;
    }

    // Displays change in HP bar
    public void RefreshHP(float curHP, float maxHP) {
        Vector3 s = fill.localScale;
        s.y = fill_bar * (curHP / maxHP);
        fill.localScale = s;
    }
}
