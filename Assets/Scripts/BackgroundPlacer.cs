using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlacer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform background;
    private Camera mainCamera;

    [Header("Logic fields")]
    float bgWidth;
    private Transform leftBg;
    private Transform middleBg;
    private Transform rightBg;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        middleBg = background;
         
        // Duplicate background
        bgWidth = background.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        leftBg = Instantiate(background, background.position + Vector3.left * bgWidth, background.rotation);
        FlipBackground(leftBg);
        rightBg = Instantiate(background, background.position + Vector3.right * bgWidth, background.rotation);
        FlipBackground(rightBg);
    }

    // Update is called once per frame
    void Update()
    {
        //SpriteRenderer middleBgSpriteRenderer = middleBg.GetChild(0).GetComponent<SpriteRenderer>();
        //
        //float halfCameraWidth = mainCamera.orthographicSize * Screen.width / Screen.height; // in world coordinates
        //float halfBgX = middleBgSpriteRenderer.bounds.size.x / 2; // in world coordinates
        //
        //float camLeftX = (transform.position + Vector3.left * halfCameraWidth).x;
        //float camRightX = (transform.position + Vector3.right * halfCameraWidth).x;
        //float bgLeftX = (middleBg.position + Vector3.left * halfBgX).x;
        //float bgRightX = (middleBg.position + Vector3.right * halfBgX).x;
        //
        //if (bgLeftX < camLeftX)
        //{
        //    Debug.Log("left");
        //    Debug.Log(middleBg.name);
        //    leftBg.position += Vector3.right * 2 * bgWidth;
        //    Transform savedBg = leftBg;
        //    leftBg = middleBg;
        //    middleBg = rightBg;
        //    rightBg = savedBg;
        //} else if (bgRightX > camRightX)
        //{
        //    Debug.Log("right");
        //    Debug.Log(middleBg.name);
        //    rightBg.position += Vector3.left * 2 * bgWidth;
        //    Transform savedBg = rightBg;
        //    rightBg = middleBg;
        //    middleBg = leftBg;
        //    leftBg = savedBg;
        //}
    }

    void FlipBackground(Transform backgroundGroup)
    {
        foreach(SpriteRenderer spriteRenderer in backgroundGroup.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.flipX = true;
        }
    }
}
