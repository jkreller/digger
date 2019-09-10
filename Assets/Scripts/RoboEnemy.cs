using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboEnemy : Humanoid {
    public float timerBeforeMove;
    public bool moveOnVisible = true;
    public float moleJumpScale = 300f;
    public bool shouldMove = true;
    public bool shouldJump;

    protected bool playerIsHittingTop;
    protected bool hittingPlayer;
    protected MolePlayer molePlayer;
    protected float isHittingSide = 1;
    protected bool isHittingGround;

    private bool move;

    /*
     * Set variables and direction of enemie
     */
    protected override void Start()
    {
        base.Start();

        molePlayer = FindObjectOfType<MolePlayer>();

        if (!isStartingToRight)
        {
            isHittingSide = -1;
        }

        if (!moveOnVisible)
        {
            move = true;
        }
    }

    /*
     * If the camera sees the enemie it starts to move
     */
    void OnBecameVisible()
    {
        if (moveOnVisible)
        {
            move = true;
        }
    }

    /*
     * Update is called once per frame
     */
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!hittingPlayer)
        {
            // timer before start moving
            if (timerBeforeMove >= 0)
            {
                timerBeforeMove -= Time.deltaTime;
            }
            else if (move && shouldMove)
            {
                // moving sidewards and change direction when hit collider
                if (isHittingSide > 0)
                {
                    Move("right");
                }
                else if (isHittingSide < 0)
                {
                    Move("left");
                }
            }

            if (move && shouldJump && isHittingGround) {
                Move("up");
                isHittingGround = false;
            }
        }
    }

    /*
     * On collision with player object
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Player"))
            {
                hittingPlayer = true;

                foreach (ContactPoint2D hit in collision.contacts)
                {
                    // check if player hits top or side and jump
                    if (hit.normal.y < 0)
                    {
                        molePlayer.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        molePlayer.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * moleJumpScale, ForceMode2D.Impulse);
                        Destroy(gameObject);
                    }
                    else
                    {
                        molePlayer.Respawn();
                    }
                }
            }
    }

    /*
     * On collision with world
     */    
    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D hit in collision.contacts)
        {
            if (hit.normal.x > .5f || hit.normal.x < -.5f)
            {
                isHittingSide = hit.normal.x;
            }

            if (hit.normal.y > .5f) {
                isHittingGround = true;
            }
        }
    }
}