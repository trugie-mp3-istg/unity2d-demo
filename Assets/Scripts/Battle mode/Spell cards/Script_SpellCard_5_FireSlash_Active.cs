using UnityEngine;

public class Script_SpellCard_5_FireSlash_Active : Script_SpellCard
{
    public GameObject slash_horizontal;
    public GameObject slash_vertical;

    private Transform target;
    public override void Use() {
        FindTarget();
        if (target != null) {
            slash_horizontal.transform.position = new Vector3(
                FindFirstObjectByType<Script_PlayerBattle>().transform.position.x, target.position.y, 0f);
            Instantiate<GameObject>(slash_horizontal);
        }
    }

    public override void MajorActiveUse() {
        FindTarget();
        if (target != null) {
            slash_horizontal.transform.position = new Vector3(
                FindFirstObjectByType<Script_PlayerBattle>().transform.position.x, target.position.y, 0f);
            slash_vertical.transform.position = new Vector3(
                target.position.x, FindFirstObjectByType<Script_PlayerBattle>().transform.position.y, 0f);
            Instantiate<GameObject>(slash_horizontal);
            Instantiate<GameObject>(slash_vertical);
        }
    }

    private void FindTarget() {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Battle_Enemy");
        float nearestDist = Mathf.Infinity;
        target = null;

        foreach (var i in targets) {
            float dist = Vector2.SqrMagnitude(i.transform.position - FindFirstObjectByType<Script_PlayerBattle>().transform.position);
            if (dist < nearestDist && i != null) {
                nearestDist = dist;
                target = i.transform;
            }
        }
    }
}
