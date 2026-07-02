using UnityEngine;

[CreateAssetMenu(fileName = "Spell Card", menuName = "Inventory/Spell Card")]
public class SC_DeckInfo : ScriptableObject
{
    public int scID;
    public string scName;
    public Sprite scSprite;
    public GameObject scPrefab;

    public ElementType scElement;
    public SC_ActivePassive scType;
    [TextArea(3, 10)]
    public string scDesc;
    [TextArea(3, 10)]
    public string scCommentary;
}

public enum SC_ActivePassive {
    Active, Passive
};
