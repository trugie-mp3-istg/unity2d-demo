using System.Collections.Generic;
using UnityEngine;

/* Elemental system:
Following the destructive cycle,
    - Bullets of a lesser element deals 50% less damage to targets of its greater element.
    - Bullets of a greater element deals 50% more damage to targets of its lesser element.
    - Clear bullets cannot clear target bullets of its greater element OR the same element.
Following the generative cycle,
    - A major active spellcard's effect is boosted if there exists 
        other spellcards of only its generative element (and its own element) in the combo hand upon release.
*/

public enum ElementType {
    Metal, Wood, Water, Fire, Earth
};

public static class ElementTypeClass {

    public static ElementType getStrongerElement(ElementType element) {
        // Sets strongerElement of element
        switch (element) {
            case ElementType.Earth:
                return ElementType.Wood;
            case ElementType.Metal:
                return ElementType.Fire;
            case ElementType.Water:
                return ElementType.Earth;
            case ElementType.Wood:
                return ElementType.Metal;
            case ElementType.Fire:
                return ElementType.Water;
            default:
                return element;
        }
    }

    public static ElementType getWeakerElement(ElementType element) {
        // Sets weakerElement of element
        switch (element) {
            case ElementType.Earth:
                return ElementType.Water;
            case ElementType.Metal:
                return ElementType.Wood;
            case ElementType.Water:
                return ElementType.Fire;
            case ElementType.Wood:
                return ElementType.Earth;
            case ElementType.Fire:
                return ElementType.Metal;
            default:
                return element;
        }
    }

    public static ElementType getSupportElement(ElementType element) {
        // Sets supportElement of element
        switch (element) {
            case ElementType.Earth:
                return ElementType.Fire;
            case ElementType.Metal:
                return ElementType.Earth;
            case ElementType.Water:
                return ElementType.Metal;
            case ElementType.Wood:
                return ElementType.Water;
            case ElementType.Fire:
                return ElementType.Wood;
            default:
                return element;
        }
    }

    // Combo requisites: the major active class is accompanied by
    // - 2 other cards (meaning all 3 slots in combo slot has to be occupied)
    // - At least 1 card of its supportive element
    // - Cards of only its own element and supportive element
    public static bool checkCombo(List<GameObject> load) {
        // If less than 3 cards in combo load
        if (load.Count < 3)
            return false;

        bool atLeast_OneSupportElementCard = false;

        // Sets major active card's element
        ElementType majorActiveElement = load[0].GetComponent<Script_SpellCard>().element;

        for (int i = 1; i < load.Count; i++) {
            ElementType curElement = load[i].GetComponent<Script_SpellCard>().element;

            // If detects a card that is not of the major's supportive element
            if (curElement != getSupportElement(majorActiveElement)) {
                // If that card is of neither the major's own nor supportive element
                if (curElement != majorActiveElement) {
                    return false;
                }
                // Else, assumes all non-supportive element cards are of the major's own element
            }

            // If detects AT LEAST ONE card of the major's supportive element
            else atLeast_OneSupportElementCard = true;
        }

        if (atLeast_OneSupportElementCard == false)
            return false;
        return true;
    }


}
 