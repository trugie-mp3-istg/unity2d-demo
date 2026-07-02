using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour {
    public static DeckManager instance;

    public List<GameObject> battleDeck = new List<GameObject>();

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}