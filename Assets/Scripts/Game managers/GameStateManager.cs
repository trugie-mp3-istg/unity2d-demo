using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public HashSet<string> flags = new HashSet<string>(); // Uses HashSet

    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return; 
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetFlag(string flag) => flags.Add(flag);
    public bool HasFlag(string flag) => flags.Contains(flag);
    public void RemoveFlag(string flag) => flags.Remove(flag);
}