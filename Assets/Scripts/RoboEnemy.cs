using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboEnemy : Humanoid {
    public float isHittingSide = 1;
    protected bool playerIsHittingTop = false;
    protected bool hittingPlayer = false;
    protected MolePlayer molePlayer;
    public float timerBeforeMove;
    public bool moveIfyouSee;
    private bool move = false;

    /*
     * Use this for initialization
     */
    protected override void Start()
    {
        base.Start();

        molePlayer = FindObjectOfType<MolePlayer>();

        if (!isStartingToRight)
        {
            isHittingSide = -1;
        }
    }

    void OnBecameVisible()
    {
        move = true;
    }

    /*
     * Update is called once per frame
     */
    protected override void Update()
    {
        base.Update();

        if (!hittingPlayer)
        {

        if (timerBeforeMove >= 0)
            {
                timerBeforeMove -= Time.deltaTime;
            }
            else
            {
            if (move)
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
        }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.collider.CompareTag("Player"))
            {
                hittingPlayer = true;

                foreach (ContactPoint2D hit in collision.contacts)
                {
                    // check if player hits top or side
                    if (hit.normal.y < 0)
                    {
                        molePlayer.enemieJump();
                        Destroy(gameObject);
                    }
                    else
                    {
                        molePlayer.Respawn();
                    }
                }
            }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D hit in collision.contacts)
        {
            if (hit.normal.x > .5f || hit.normal.x < -.5f)
            {
                isHittingSide = hit.normal.x;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            hittingPlayer = false;
        }
    }
}