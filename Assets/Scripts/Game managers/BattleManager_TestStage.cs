using UnityEngine;

public class BattleManager_TestStage : BattleManager {
    public int kill_KPI = 6;
    private int killed = 0;

    public override void OnKill() {
        killed++;
        CheckWinCondition();
    }

    protected override void CheckWinCondition() {
        if (killed >= kill_KPI)
            OnBattleEnd();
    }
}
