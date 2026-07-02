using UnityEngine;
using System.Collections;

public abstract class Enemy_AttackBase : MonoBehaviour
{
    public abstract IEnumerator Execute();
}
