using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShop : MonoBehaviour
{
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void pressButton()
    {
        animator.SetInteger("AnimState", 1);
    }

    void releaseButton()
    {
        animator.SetInteger("AnimState", 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressButton();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            releaseButton();
        }
    }
}
