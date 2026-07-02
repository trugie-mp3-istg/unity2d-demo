using UnityEngine;

public class Script_BulletSpellcard1_CosmeticPulse : MonoBehaviour {
    private float pulseSpeed = 2f;
    private float pulseMin = 0.85f;
    private float pulseMax = 1.15f;

    void Update() {
        // Bullet pulses by pingpong function
        // A purely cosmetic script
        float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        t = Mathf.Round(t * 3f) / 3f; // snaps to 0, 0.33, 0.67, 1
        float scale = Mathf.Lerp(pulseMin, pulseMax, t);
        transform.localScale = new Vector3(scale, scale, 0f);
    }
}