﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleAnimation : MonoBehaviour {

    Animator animator;
    public float speed;
    SpriteRenderer spriteRenderer;

	void Start () {

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }
	
	// Update is called once per frame
	void Update () {


        float move = Input.GetAxis("Horizontal");

        if(move > 0){

            spriteRenderer.flipX = false;
            transform.Translate(Vector2.right * Time.deltaTime*speed);
            animator.SetBool("move", true);


        }
        else if(move < 0){

            transform.Translate(Vector2.left * Time.deltaTime*speed);
            animator.SetBool("move", true);
            spriteRenderer.flipX = true;
           
        }
        else{

            animator.SetBool("move", false);

        }

		
	}


}
