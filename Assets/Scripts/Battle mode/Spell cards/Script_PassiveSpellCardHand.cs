using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Script_PassiveSpellCardHand : MonoBehaviour
{
    float spacing = 80f;
    public List<GameObject> deck = new List<GameObject>();

    void Start() {
        // LINQ partition from Script_DeckManager
        deck.Clear();
        deck = DeckManager.instance.battleDeck.Where(go => !go.GetComponent<Script_SpellCard>().IsActive()).ToList();

        for (int i = 0; i < deck.Count; i++) {
            GameObject card = Instantiate(deck[i]);
            card.transform.SetParent(transform, false);
            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * spacing, 0f);
            card.GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0f);

            Color dimmed = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            // Affects both the card and its children gameObject (i.e. the frame)
            foreach (var img in card.GetComponentsInChildren<Image>()) {
                img.color = dimmed;
            }

            // Triggers passive spell cards' effects
            card.GetComponent<Script_SpellCard>().Use();
        }
    }
}
