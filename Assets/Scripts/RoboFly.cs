using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboFly : RoboEnemy {
    public bool moveLeftAndRight = false;
    public Vector2 flyingBorders = new Vector2(-0.2f, 0.2f);
    public float flyingRandomizeFactor = 0.2f;

    private Vector2 randomFlyingBorders;

    protected override void Start() {
        base.Start();

        randomFlyingBorders = flyingBorders;
    }

    protected override void Update() {
        // if robo should also move to left and right
        if (moveLeftAndRight) {
            base.Update();
        }

        var upperBorderHitted = false;
        var lowerBorderHitted = false;

        if (transform.position.y <= startingPoint.y + randomFlyingBorders.x) {
            lowerBorderHitted = true;
            Move("up");
            animator.SetInteger("AnimState", 1);
        } else if (transform.position.y >= startingPoint.y + randomFlyingBorders.y) {
            upperBorderHitted = true;
            animator.SetInteger("AnimState", 0);
        }

        // randomize flying height
        if (upperBorderHitted) {
            var deltaRange = flyingBorders.y * flyingRandomizeFactor;
            randomFlyingBorders.y += Random.Range(-deltaRange, deltaRange);
            upperBorderHitted = false;
        } else if (lowerBorderHitted) {
            var deltaRange = flyingBorders.x * flyingRandomizeFactor;
            randomFlyingBorders.x += Random.Range(-deltaRange, deltaRange);
            lowerBorderHitted = false;
        }
    }
}