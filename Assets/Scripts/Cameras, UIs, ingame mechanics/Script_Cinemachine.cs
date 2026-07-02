using Cinemachine;
using UnityEngine;

public class Script_Cinemachine : MonoBehaviour
{
    private CinemachineVirtualCamera cmVirtualCamera;
    void Awake() {
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Start() {
        CameraFollow();
    }

    // Finds the player and follows them
    // Works in both overworld and battle mode, with overworld player taking priority
    public void CameraFollow() {
        if (Script_Player.instance != null && Script_Player.instance.gameObject.activeInHierarchy) {
            cmVirtualCamera.Follow = Script_Player.instance.transform;
        }
        else if (Script_PlayerBattle.instance != null && Script_PlayerBattle.instance.gameObject.activeInHierarchy) {
            cmVirtualCamera.Follow = Script_PlayerBattle.instance.transform;
        }
        else
            Debug.LogWarning("ERROR (Developer-made message): No player found");
    }
}
