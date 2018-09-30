using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    public bool simulateMobile;

    [HideInInspector]
    public float movingHorizontal { get; set; }
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool isDigging { get; set; }
    [HideInInspector]
    public bool isLookingDown { get; set; }
    [HideInInspector]
    public bool isLookingUp { get; set; }
    [HideInInspector]
    public bool pause;

    private Touch startTouch;
    private Touch endTouch;
    private GameObject mobileControlsObject;
    private bool onMobileDevice;

    /*
     * Getter and setter for isJumping property
     */
    public bool IsJumping
    {
        get { return isJumping; }
        set
        {
            if (value)
            {
                StartCoroutine(SetJumpingFalse());
            }
            isJumping = value;
        }
    }

    /*
     * Getter and setter for pause property
     */
    public bool Pause
    {
        get { return pause; }
        set
        {
            if (value)
            {
                StartCoroutine(SetPauseFalse());
            }
            pause = value;
        }
    }

    /*
     * Coroutine for set jumping false 
     */
    public IEnumerator SetJumpingFalse()
    {
        yield return new WaitForFixedUpdate();
        isJumping = false;
    }

    /*
     * Coroutine for set pause false
     */
    public IEnumerator SetPauseFalse()
    {
        yield return new WaitForEndOfFrame();
        pause = false;
    }

    /*
     * Use this for initialization
     */
    private void Start()
    {
        #if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            onMobileDevice = true;
        #else
            if (simulateMobile) {
                onMobileDevice = true;
            }
        #endif

        mobileControlsObject = GameObject.Find("MobileControls");
    }

    /*
     * Update is called once per frame
     */
    private void Update()
    {
        if (!onMobileDevice)
        {
            // controls for not mobile devices

            // disable mobile controls
            if (mobileControlsObject)
            {
                mobileControlsObject.SetActive(false);
            }

            // moving
            movingHorizontal = Input.GetAxisRaw("Horizontal");

            // jumping
            isJumping = Input.GetButtonDown("Jump");

            // digging
            isDigging = Input.GetKey("left shift");

            // looking up
            isLookingUp = Input.GetKey("up");

            // looking up
            isLookingDown = Input.GetKey("down");

            // pause game
            pause = Input.GetKeyDown(KeyCode.Escape);
        }
        else
        {
            // controls for mobile devices

            if (!mobileControlsObject)
            {
                // controls for menu
                var touchOrigin = -Vector2.one;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    var screenMiddleX = Screen.width / 2;
                    if (touch.position.x < screenMiddleX && movingHorizontal <= 0f)
                    {
                        movingHorizontal = -1f;
                    }
                    else if (touch.position.x > screenMiddleX && movingHorizontal >= 0f)
                    {
                        movingHorizontal = 1f;
                    }

                    if (touch.phase == TouchPhase.Began)
                    {
                        startTouch = touch;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        movingHorizontal = 0;

                        endTouch = touch;

                        // if swipe up
                        if (endTouch.position.y > startTouch.position.y + Screen.height / 16)
                        {
                            IsJumping = true;
                        }
                    }
                }
            }
        }
    }
}
