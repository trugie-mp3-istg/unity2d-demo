using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Deck: contains (the information of) every (active) card the player can use this round
// Hand: contains cards currently available to the player to use; using or loading a card removes it from hand
// Load: for combo
public class Script_ActiveSpellCardHand : MonoBehaviour
{
    float spacing = 120f;
    int startingHand = 3;
    int maxHand = 5;
    int selectedIndex = 0;

    public Transform majorActiveSlot;
    public Transform supportSlot1;
    public Transform supportSlot2;
    private int maxLoad = 3;
    private int curLoad = 0;
    private GameObject loadedCard;

    public List<GameObject> hand = new List<GameObject>();
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> load = new List<GameObject>();

    private HashSet<GameObject> inUse = new HashSet<GameObject>();

    public float drawInterval = 4f;
    private float drawTimer = 0f;

    void Start()
    {
        // LINQ partition from Script_DeckManager
        deck.Clear();
        deck = DeckManager.instance.battleDeck.Where(go => go.GetComponent<Script_SpellCard>().IsActive()).ToList();

        if (startingHand > deck.Count) {
            startingHand = deck.Count;
        }

        if (maxHand > deck.Count) {
            maxHand = deck.Count;
        }

        for (int i = 0; i < startingHand; i++) {
            GameObject template = deck[i];
            inUse.Add(template);
            GameObject card = Instantiate(template);
            card.GetComponent<Script_SpellCard>().template = template;
            AddCard(card);
        }
    }

    void Update()
    {
        // Drawing card timer only plays if hand is not full        
        if (hand.Count < maxHand) {
            drawTimer += Time.deltaTime;
        }

        // After every few seconds, draws a card
        if (drawTimer >= drawInterval) {
            drawTimer = 0f;

            if (hand.Count < maxHand && deck.Count > 0) {
                // Specifically, draws a card that isn't in hand at the moment
                List<GameObject> available = deck.Where(c => !inUse.Contains(c)).ToList();
                if (available.Count == 0) return;

                int randomIndex = UnityEngine.Random.Range(0, available.Count);
                GameObject template = available[randomIndex];
                inUse.Add(template);

                GameObject card = Instantiate(template);
                card.GetComponent<Script_SpellCard>().template = template;
                AddCard(card);
                ;
            }
        }

        // Independent from hand, so this should be put above the return call below
        if (!Script_IngameMenu.isMenuActive && Input.GetKeyDown(PLAYER_INPUT.spellcard_use)) {
            // If load is not empty, fires everything from load
            if (curLoad > 0) {
                // Checks if combo requisites are reached
                bool activateCombo = ElementTypeClass.checkCombo(load);

                // If requisites are reached, the major in load gets a powerup
                if (activateCombo) {
                    UseMajorActiveCard(load[0]);
                    for (int i = 1; i < curLoad; i++) UseCard(load[i]);
                }

                // If requisites are not reached, just plays all cards in load
                else for (int i = 0; i < curLoad; i++) {
                    UseCard(load[i]);
                }
                curLoad = 0;
                load.Clear();
            }

            // If load is empty, uses the selected Active spellcard
            // Try-catch block handles ArrayOutOfBounds error where upon using all spell cards,
            // selectedIndex doesn't exist when cards are redrawn to hand
            else {
                try {
                    UseCard(hand[selectedIndex]);
                }
                catch (ArgumentOutOfRangeException) {
                    selectedIndex = 0;
                    RefreshLayout();
                }
            }
        }

        if (hand.Count <= 0)
        return;

        if (!Script_IngameMenu.isMenuActive) {

            // Loops between Active spell cards in hand
            if (Input.GetKeyDown(PLAYER_INPUT.spellcard_switch)) {
                selectedIndex = (selectedIndex + 1) % hand.Count;
                RefreshLayout();
            }

            // Loads card
            if (Input.GetKeyDown(PLAYER_INPUT.spellcard_load)) {
                try {
                    loadedCard = hand[selectedIndex];
                    LoadCard(loadedCard);
                }
                catch (ArgumentOutOfRangeException) {
                    selectedIndex = 0;
                    RefreshLayout();
                }
            }
        } 
    }

    void AddCard(GameObject card) {
        if (hand.Count >= maxHand) return;

        card.transform.SetParent(transform, false);
        card.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        hand.Add(card);
        RefreshLayout();
    }

    void UseCard(GameObject card) {
        card.GetComponent<Script_SpellCard>().Use();
        RemoveCard(card);
    }

    void UseMajorActiveCard(GameObject card) {
        card.GetComponent<Script_SpellCard>().MajorActiveUse();
        RemoveCard(card);
    }

    void RemoveCard(GameObject card) {
        inUse.Remove(card.GetComponent<Script_SpellCard>().template);
        hand.Remove(card);
        Destroy(card);
        selectedIndex = Mathf.Clamp(selectedIndex, 0, hand.Count - 1);

        RefreshLayout();
    }
    
    // Refreshes the spacing among cards
    // Is called every time a card is added to or removed from hand.
    // Also keeps track of selected cards through index looping
    void RefreshLayout() {
        if (hand.Count == 0) return;
        int n = hand.Count;
        for (int i = 0; i < n; i++) {
            float x = (i - (n - 1) / 2f) * spacing;
            float y = (i == selectedIndex) ? 50f : -5f;
            float r = (i == selectedIndex) ? 1f : 0.7f;
            float g = (i == selectedIndex) ? 1f : 0.7f;
            float b = (i == selectedIndex) ? 1f : 0.7f;
            float a = (i == selectedIndex) ? 1f : 0.7f;
            hand[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            hand[i].GetComponent<Image>().color = new Color(r, g, b, a);
        }
    }

    void LoadCard(GameObject card) {
        if (curLoad >= maxLoad) {
            return;
        }
        hand.Remove(card);

        // The first card counts as the major active spellcard
        if (curLoad == 0) card.transform.SetParent(majorActiveSlot, false);
        else if (curLoad == 1) card.transform.SetParent(supportSlot1, false);
        else if (curLoad == 2) card.transform.SetParent(supportSlot2, false);
        card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        RefreshLayout();

        load.Add(card);
        curLoad++;
    }
}
