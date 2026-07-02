using UnityEngine;

public class Script_SpellCard : MonoBehaviour {
    public bool isActive;

    public bool IsActive() {
        return isActive;
    }

    public ElementType element;

    public GameObject template; // Intentionally empty in serial field, don't delete

    public virtual void Use() {
    }

    // Special effects for major active spellcards that are boosted in a combo
    public virtual void MajorActiveUse() {
    }
}

// In case this is revisited for debugging purposes,
// spell card elements are only for combo boosting.
// Bullets' elements are set directly and manually in prefab.