using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [HideInInspector]
    public bool isShaking;
    [HideInInspector]
    public bool isOpen;
    public float secondsToOpen = 2;
    public float secondsAfterOpening = 1;

    private Animator animator;
    private int framesShaking;
    private GameLogic gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    // Fixed Update is called once per frame
    void FixedUpdate()
    {
        if (!isShaking && !isOpen) {
            StopShakingChest();
        }
    }

    public void StopShakingChest()
    {
        isShaking = false;
        animator.SetInteger("AnimState", 0);
    }

    public void TryOpenChest() {
        if (framesShaking * Time.deltaTime < secondsToOpen) {
            isShaking = true;
            animator.SetInteger("AnimState", 1);
            framesShaking++;
        } else {
            StartCoroutine(OpenChest());
        }
    }

    private IEnumerator OpenChest() {
        isShaking = false;
        isOpen = true;
        animator.SetInteger("AnimState", 2);

        yield return new WaitForSeconds(secondsAfterOpening);

        gameLogic.nextScene();
    }
}