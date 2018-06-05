using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 150f;
    public float jumpSpeed = 500f;
    public int maxSpeed = 60;

    private Rigidbody2D body2D;
    private SpriteRenderer renderer2D;
    private PlayerController controller;
    private Animator animator;

    private bool grounded = false;
    public Transform groundCheck;
    private float groundRadius = 0.2f;
    public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
        body2D = GetComponent<Rigidbody2D>();
        renderer2D = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        var absVelX = Mathf.Abs(body2D.velocity.x);

        var forceX = 0f;
        var forceY = 0f;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        if (controller.moving.x != 0) {
            if (absVelX < maxSpeed) {
                forceX = speed * controller.moving.x;
                renderer2D.flipX = controller.moving.x < 0;
            }

            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }

        if (controller.moving.y != 0) {
            if (grounded) {
                forceY = jumpSpeed * controller.moving.y;
            }
        }

        body2D.AddForce(new Vector2(forceX, forceY));
	}
}
