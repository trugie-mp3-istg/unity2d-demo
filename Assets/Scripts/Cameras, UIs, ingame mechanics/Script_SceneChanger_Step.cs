using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_SceneChanger_Step : MonoBehaviour
{
    public string nextScene;
    public Animator fadeAnim;
    public float fadeTime = 0.5f;
    public bool keepPlayerX = false;
    public bool keepPlayerY = false;
    public Vector2 newPlayerPosition;
    private Transform player;
    

    private void OnTriggerEnter2D(Collider2D collision) {
        // Triggers if player collides with a scene changer
        if (collision.gameObject.tag == "Player") {
            fadeAnim.Play("fade_to_black");
            player = collision.transform;
            StartCoroutine(FadeDelay());
        }

        IEnumerator FadeDelay() {
            yield return new WaitForSeconds(fadeTime);
            SceneManager.LoadScene(nextScene);

            // Preserves the player's X/Y coordinates. If false, teleports player to a set destination instead
            if (keepPlayerX)
                newPlayerPosition.x = player.position.x;
            if (keepPlayerY)
                newPlayerPosition.y = player.position.y;

            player.position = newPlayerPosition;
        }
    }
}
