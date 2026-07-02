using UnityEngine;

public class Script_SpellCard_2_HealQuarter_Active : Script_SpellCard
{
    private Script_PlayerBattleMain player;
    void Start() {
        player = FindFirstObjectByType<Script_PlayerBattleMain>();
    }
    
    public override void Use() {
        player.getHealed(0.25f);
    }

    public override void MajorActiveUse() {
        Use();
        Use();
    }
}
