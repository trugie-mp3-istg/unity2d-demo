using UnityEngine;

public class UNUSED_Script_SpellCard_5_FireSlash_Active_UNUSED : Script_SpellCard 
{
    public GameObject uiAttackPrefab;
    public RectTransform uiParent;

    public override void Use() {
        uiParent = GameObject.Find("AttackCanvas").GetComponent<RectTransform>();
        Script_PlayerBattle.instance.GetComponent<Script_PlayerBattle>().StartCoroutine(Fire());
    }

    public override void MajorActiveUse() {
    }

    private System.Collections.IEnumerator Fire() {
        GameObject attack = Instantiate(uiAttackPrefab);
        attack.transform.SetParent(uiParent, false);
        yield return new WaitForSeconds(0.3f);
        Destroy(attack);
    }
}
