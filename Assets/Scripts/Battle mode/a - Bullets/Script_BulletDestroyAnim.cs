using UnityEngine;

// This script handles bullet death animation's timeout

public class Script_BulletDestroyAnim : MonoBehaviour {

    void Update() {
        if (!GetComponent<Animator>().IsInTransition(0) &&
            GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            Destroy(gameObject);
        }
    }
}
