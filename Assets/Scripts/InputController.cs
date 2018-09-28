using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    [HideInInspector]
    public float movingHorizontal;
    [HideInInspector]
    public bool isJumping { get; set; }
    [HideInInspector]
    public bool isDigging { get; set; }
    [HideInInspector]
    public bool isLookingDown;
    [HideInInspector]
    public bool isLookingUp;
    [HideInInspector]
    public bool pause;
    private Joystick joystick;
    private GameObject mobileControlsObject;
    private bool onMobileDevice;


    /*
     * Use this for initialization
     */
    private void Start()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            onMobileDevice = true;
#endif

        joystick = FindObjectOfType<Joystick>();

        mobileControlsObject = GameObject.Find("MobileControls");

    }

    /*
     * Update is called once per frame
     */
    void Update()
    {
        if (!pause)
        {
            if (!onMobileDevice)
            {
                // controls for not mobile devices

                // disable mobile controls
                if (joystick && mobileControlsObject)
                {
                    joystick.enabled = false;
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
            }
            else
            {
                // controls for mobile devices

                if (joystick)
                {
                    // moving
                    movingHorizontal = (joystick.Vertical < 0.3 && joystick.Vertical > -0.3) ? joystick.Horizontal : 0;

                    // looking up
                    isLookingUp = joystick.Vertical > 0;

                    // looking up
                    isLookingDown = joystick.Vertical < 0;
                }
            }
        }
    }
}
