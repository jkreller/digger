using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MolePlayer : Humanoid {
    public Tilemap digableMap;
    public float checkerRadius = 5;
    private GameLogic gameLogic;
    private GameObject logic;
    private bool isGrounded = false;
    private bool isHittingLeft = false;
    private bool isHittingRight = false;
    private LayerMask worldLayer;
    private Transform checkerBelowLeft;
    private Transform checkerBelowRight;
    private Transform checkerLeftUpper;
    private Transform checkerLeftLower;
    private Transform checkerRightUpper;
    private Transform checkerRightLower;
    private bool onMobileDevice = false;
    private Touch startTouch;
    private Touch endTouch;
    private List<string> movingDirections = new List<string>();

    // Use this for initialization
    protected override void Start () {
        base.Start();

        #if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            onMobileDevice = true;
        #endif

        worldLayer = LayerMask.GetMask("World");

        checkerBelowLeft = GameObject.Find("CheckerBelowLeft").transform;
        checkerBelowRight = GameObject.Find("CheckerBelowRight").transform;
        checkerLeftUpper = GameObject.Find("CheckerLeftUpper").transform;
        checkerLeftLower = GameObject.Find("CheckerLeftLower").transform;
        checkerRightUpper = GameObject.Find("CheckerRightUpper").transform;
        checkerRightLower = GameObject.Find("CheckerRightLower").transform;

        logic = GameObject.Find("GameLogic");
        if (logic) {
            gameLogic = logic.GetComponent<GameLogic>();
        }
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        // checking sides for not getting stuck
        var isHittingLeftUpper = Physics2D.OverlapCircle(checkerLeftUpper.position, checkerRadius, worldLayer);
        var isHittingLeftLower = Physics2D.OverlapCircle(checkerLeftLower.position, checkerRadius, worldLayer);
        var isHittingRightUpper = Physics2D.OverlapCircle(checkerRightUpper.position, checkerRadius, worldLayer);
        var isHittingRightLower = Physics2D.OverlapCircle(checkerRightLower.position, checkerRadius, worldLayer);

        isHittingLeft = isHittingLeftLower || isHittingLeftUpper;
        isHittingRight = isHittingRightLower || isHittingRightUpper;

        // checking if player is grounded
        var isGroundedLeft = Physics2D.OverlapCircle(checkerBelowLeft.position, checkerRadius, worldLayer);
        var isGroundedRight = Physics2D.OverlapCircle(checkerBelowRight.position, checkerRadius, worldLayer);

        isGrounded = isGroundedLeft || isGroundedRight;

        movingDirections = new List<string>();
        
        float movingHorizontal = 0f;
        bool isJumping = false;

        if (onMobileDevice)
        {
            // controls for mobile devices
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
                    endTouch = touch;

                    // if swipe up
                    if (endTouch.position.y > startTouch.position.y + Screen.height / 16)
                    {
                        isJumping = true;
                    }
                }
            }
        }
        else
        {
            // controls for not mobile devices
            // moving
            movingHorizontal = Input.GetAxis("Horizontal");

            // jumping
            isJumping = Input.GetButtonDown("Jump");
        }

        if (movingHorizontal > 0) {
            if (!isHittingRight) {
                movingDirections.Add("right");
            }
            animator.SetInteger("AnimState", 1);

        } else if (movingHorizontal < 0) {
            if (!isHittingLeft){
                movingDirections.Add("left");
            }
            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }

        // jumping
        if (isJumping && isGrounded) {
            movingDirections.Add("up");
        }

        // digging
        if (Input.GetKey("left shift")) {
            Dig();
        }

        // move player
        Move(movingDirections);
    }

    protected void Dig() {
        var digTileDistance = 32.0f;
        var digTileHeight = 32.0f;
        var grid = digableMap.GetComponentInParent<Grid>();
        var tilesToDestroy = new ArrayList();

        if (!grid) {
            return;
        }

        if (Input.GetKey("down"))
        {
            // dig down
            var tilePosition = transform.position;

            tilePosition.y -= digTileHeight;
            tilesToDestroy.Add(tilePosition);
        }
        else
        {
            // dig to side
            animator.SetInteger("AnimState", 2);

            var tilePosition = transform.position;

            if (!spriteRenderer.flipX)
            {
                tilePosition.x += digTileDistance;
            }
            else
            {
                tilePosition.x -= digTileDistance;
            }

            tilesToDestroy.Add(tilePosition);

            tilePosition.y += digTileHeight;
            tilesToDestroy.Add(tilePosition);
        }

        // destroy selected tiles
        foreach (Vector3 position in tilesToDestroy) {
            var positionCell = grid.WorldToCell(position);
            digableMap.SetTile(positionCell, null);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("coin")) {
            Destroy(other.gameObject);
            gameLogic.addDiamonds(1);
        }

        if (other.gameObject.name == "LevelEnd") {
            Respawn();
        }

    }

    public void enemieJump() {
        movingDirections.Add("enemieJump");
        Move(movingDirections);
    }

	
}
