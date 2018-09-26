using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Humanoid : MonoBehaviour {
    public float speed = 80;
    public float upwardsSpeed = 500;
    public Vector2 startingPoint = Vector2.zero;
    public bool isStartingToRight = true;

    protected Rigidbody2D body2D;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    // Use this for initialization
    protected virtual void Start() {
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (!isStartingToRight) {
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    protected virtual void Update() {
        // avoid sliding
        Vector2 easeVelocity = body2D.velocity;
        easeVelocity.x *= 0.7f;

        body2D.velocity = easeVelocity;
    }

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

    protected void Move(string direction) {
        Move(new List<string>() { direction });
    }

    public void Respawn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}