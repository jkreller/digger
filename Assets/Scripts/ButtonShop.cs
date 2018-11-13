using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShop : MonoBehaviour
{
    protected Animator animator;
    protected bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetButtonPressed() {
        animator.SetInteger("AnimState", 2);
        // todo Buy here!
    }

    void SetButtonReleased()
    {
        animator.SetInteger("AnimState", 0);
        isPressed = false;
    }

    void PressButton()
    {
        if (!isPressed) {
            isPressed = true;
            animator.SetInteger("AnimState", 1);
        }
    }

    void ReleaseButton()
    {
        if (isPressed)
        {
            animator.SetInteger("AnimState", 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D hit in other.contacts)
            {
                // if hit on top
                if (hit.normal.y < -0.9f) {
                    PressButton();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ReleaseButton();
        }
    }
}
