using UnityEngine;

// For beam attacks that aren't treated as a world-space object, but rather fixed (even if partially so) to the camera
// No collision with terrain
public class Script_BeamUI : MonoBehaviour {
    public enum Owner {
        Player, Enemy
    }
    public enum Category {
        Normal, Clear
    }

    public Owner owner;
    public Category category = Category.Normal;
    public ElementType element;

    public float lifetime = 1f;
    public float rotation = 0f;
    public float damage = 1f;

    private float timer = 0f;
    public GameObject deathAnimObject;

    [System.NonSerialized]
    public ElementType strongerElement;

    void Awake() {
        // Sets strongerElement and weakerElement of element
        strongerElement = ElementTypeClass.getStrongerElement(element);
    }
    void Update() {
        if (timer > lifetime) {
            // Spawns bullet destroy animation
            if (deathAnimObject != null) {
                Instantiate(deathAnimObject, transform.position, transform.rotation);
            }
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
    }
}
