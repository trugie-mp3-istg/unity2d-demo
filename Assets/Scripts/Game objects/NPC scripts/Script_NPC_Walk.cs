using System.Collections;
using UnityEngine;

public class Script_NPC_Walk : MonoBehaviour
{
    public enum walkModeList { stationary, closedLoop, retread }
    public walkModeList walkMode = walkModeList.stationary;
    public Vector2[] walkpoints;
 
    public float speed = 2f; // changeable
    public float pauseTime = 1f; // changeable

    private Vector2 target;
    private int curWalkpoint;
    private bool isPaused = false;
    private bool isTreadingBack = false;
    
    private Rigidbody2D npc_rb;
    public Animator npc_anim;
    
    
    void Start() {
        npc_rb = GetComponent<Rigidbody2D>();
        target = walkpoints[curWalkpoint];
    }

    void Update() {
        // Stops the NPC while in pause state (talking, etc.)
        if (isPaused) {
            CharacterStops();
            return;
        }

        // Movement
        if (walkMode != walkModeList.stationary) {
            Vector2 direction = ((Vector3)target - transform.position).normalized;
            npc_rb.linearVelocity = direction * speed;
            npc_anim.SetFloat("horizontal", direction.x);
            npc_anim.SetFloat("vertical", direction.y);
        }

        // Sets up another walkpoint after having reached one
        if (Vector2.Distance(transform.position, target) < 0.1f) {
            StartCoroutine(setWalkPoint());
        }
    }

    public void CharacterStops() {
        npc_rb.linearVelocity = Vector2.zero;
        npc_anim.SetFloat("horizontal", 0f);
        npc_anim.SetFloat("vertical", 0f);
    }

    // Sets walk points for NPCs depending on their walk mode
    IEnumerator setWalkPoint()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        
        // Walk mode: NPC is stationary
        if (walkMode == walkModeList.stationary) curWalkpoint = 0;

        // Walk mode: NPC walks in a closed loop
        else if (walkMode == walkModeList.closedLoop) {
            curWalkpoint = (curWalkpoint + 1) % walkpoints.Length;
        }

        // Walk mode: NPC treads back path upon reaching end of loop
        else if (walkMode == walkModeList.retread) {
            if (curWalkpoint + 1 == walkpoints.Length) isTreadingBack = true;
            if (curWalkpoint == 0) isTreadingBack = false; 
            if (isTreadingBack == true) curWalkpoint--;
            else curWalkpoint++;
        }

        target = walkpoints[curWalkpoint];
        isPaused = false;
    }
}
