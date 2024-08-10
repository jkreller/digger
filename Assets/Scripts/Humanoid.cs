using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Humanoid : MonoBehaviour {
    public float speed = 80;
    public float upwardsSpeed = 500;
    public Vector2 startingPoint = Vector2.zero;
    public bool isStartingToRight = true;

    protected Rigidbody2D body2D;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    /*
     * initializing abstract humanoid
     */
    protected virtual void Start() {
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (!isStartingToRight) {
            spriteRenderer.flipX = true;
        }

        startingPoint = transform.position;
    }

    protected virtual void FixedUpdate() {
    }

    protected virtual void Update() {
        // avoid sliding
        Vector2 easeVelocity = body2D.linearVelocity;
        easeVelocity.x *= 0.7f;
		
        body2D.linearVelocity = easeVelocity;
    }

    /*
     * moving humanoid
     */
    protected void Move(List<string> directions) {
        // force which is going to be applied
        var force = Vector2.zero; 

        // check to which site object should be moved
        foreach (string direction in directions) {
            switch (direction) {
                case "left":
                    force.x = -speed;
                    spriteRenderer.flipX = true;
                    break;
                case "right":
                    force.x = speed;
                    spriteRenderer.flipX = false;
                    break;
                case "up":
                    force.y = upwardsSpeed;
                    break;
                case "down":
                    force.y = -upwardsSpeed;
                    break;
            }
        }

        // apply force
        body2D.AddForce(force, ForceMode2D.Impulse);
    }

    /*
     * start move with string of direction
     */
    protected void Move(string direction) {
        Move(new List<string>() { direction });
    }

    /*
     * respawn humanoid
     */
    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}