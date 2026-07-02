using UnityEngine;
using System.Collections;

public class Enemy_AttackController : MonoBehaviour
{
    private Enemy_AttackBase[] attacks;

    void Start() {
        attacks = GetComponentsInChildren<Enemy_AttackBase>();
        StartCoroutine(AttackRandom());
    }

    IEnumerator AttackRandom() {
        while (true) {
            var pick = attacks[Random.Range(0, attacks.Length)];
            yield return StartCoroutine(pick.Execute());
        }
    }
}
