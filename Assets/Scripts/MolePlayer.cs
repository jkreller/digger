using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MolePlayer : Humanoid {
    public float checkerRadius = 5;

    private Tilemap digableMap;
    private GameLogic gameLogic;
    private InputController inputController;
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
    private List<string> movingDirections = new List<string>();
    private bool hasFinished;
    private GameObject chest;
    private Chest chestClass;
    private bool isHittingChest;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        var digableGameObject = GameObject.Find("Digable");
        if (digableGameObject) {
            digableMap = digableGameObject.GetComponent<Tilemap>();
        }

        chest = GameObject.Find("Chest");
        if (chest) {
            chestClass = chest.GetComponent<Chest>();
        }

        worldLayer = LayerMask.GetMask("World");

        checkerBelowLeft = GameObject.Find("CheckerBelowLeft").transform;
        checkerBelowRight = GameObject.Find("CheckerBelowRight").transform;
        checkerLeftUpper = GameObject.Find("CheckerLeftUpper").transform;
        checkerLeftLower = GameObject.Find("CheckerLeftLower").transform;
        checkerRightUpper = GameObject.Find("CheckerRightUpper").transform;
        checkerRightLower = GameObject.Find("CheckerRightLower").transform;

        var logic = GameObject.Find("GameLogic");
        if (logic) {
            gameLogic = logic.GetComponent<GameLogic>();
            inputController = gameLogic.GetComponent<InputController>();
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

        if (inputController.movingHorizontal > 0) {
            if (!isHittingRight) {
                movingDirections.Add("right");
            }
            animator.SetInteger("AnimState", 1);

        } else if (inputController.movingHorizontal < 0) {
            if (!isHittingLeft){
                movingDirections.Add("left");
            }
            animator.SetInteger("AnimState", 1);
        } else {
            animator.SetInteger("AnimState", 0);
        }

        // jumping
        if (inputController.isJumping && isGrounded) {
            movingDirections.Add("up");
        }

        // digging
        if (inputController.isDigging)
        {
            Dig();
        } else if (chestClass && chestClass.isShaking) {
            chestClass.isShaking = false;
        }

        // move player
        Move(movingDirections);
    }

    protected void Dig() {
        var digTileDistance = 32.0f;
        var digTileHeight = 32.0f;

        Grid grid = null;
        if (digableMap) {
            grid = digableMap.GetComponentInParent<Grid>();
        }

        var tilesToDestroy = new ArrayList();
        var tilePosition = transform.position;

        if (inputController.isLookingDown)
        {
            // dig down
            tilePosition.y -= digTileHeight;
            tilesToDestroy.Add(tilePosition);

            // open chest if under mole
            if (isHittingChest && isGrounded && !isHittingLeft && !isHittingRight) {
                chestClass.TryOpenChest();
            }
        } else if (inputController.isLookingUp) {
            // dig up
            animator.SetInteger("AnimState", 3);

            tilePosition.y += 2 * digTileHeight;
            tilesToDestroy.Add(tilePosition);
        } else {
            // dig to side
            animator.SetInteger("AnimState", 2);

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

            // open chest if on side of mole
            if (isHittingChest && isHittingLeft || isHittingRight)
            {
                chestClass.TryOpenChest();
            }
        }

        if (grid)
        {
            foreach (Vector3 position in tilesToDestroy) {
                var positionCell = grid.WorldToCell(position);
                digableMap.SetTile(positionCell, null); 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("coin")) {
            gameLogic.addDiamonds(other.gameObject.GetComponent<Diamond>().value);
            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "LevelEnd") {
            if (!hasFinished) {
                Respawn();
            }
        }

        if (other.gameObject.CompareTag("Finish")) {
            hasFinished = true;
            gameLogic.nextScene();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Chest")
        {
            isHittingChest = true;
        }
    }
}
