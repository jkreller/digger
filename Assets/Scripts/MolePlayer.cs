using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MolePlayer : Humanoid {
    public float checkerRadius = 5;
    public string costume;

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
    private string movingControlled;

    private bool hasFinished;
    private GameObject chest;
    private Chest chestClass;
    private bool isHittingChest;

    private Sprite[] subSprites;
    private CostumeAssortment costumeAssortment;

    private bool redPotionActive;
    private bool canDoubleJump = true;

    /*
     * initializing Moleplayer and all of the needed classes
     */
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

        costumeAssortment = FindObjectOfType<CostumeAssortment>();
        costume = loadGame.safeData.currentCostume;
        LoadSubSprites();
    }
	
    /*
     * overwriting the update of the Humanoid class
     */
	protected override void FixedUpdate () {
        base.FixedUpdate();

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

        if (inputController.movingHorizontal > 0 || movingControlled == "right") {
            if (!isHittingRight)
            {
                movingDirections.Add("right");
            }
            else if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }

        } else if (inputController.movingHorizontal < 0 || movingControlled == "left") {
            if (!isHittingLeft)
            {
                movingDirections.Add("left");
            }
            else if (!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
        } else {
            animator.SetInteger("AnimState", 0);
        }
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        // jumping
        if (inputController.isJumping && isGrounded) {
            movingDirections.Add("up");
        }
        // red potion double jump
        else if(inputController.isJumping && !isGrounded)
        {
            if (redPotionActive)
            {
                
                if (canDoubleJump)
                {
                    Debug.Log("test");
                    movingDirections.Add("up");
                    canDoubleJump = false;
                }

            }
        }
            
        // digging
        if (inputController.isDigging)
        {
            Dig();
        } else if (chestClass && chestClass.isShaking) {
            chestClass.isShaking = false;
        }

        // move player
        if (movingDirections.Count > 0)
        {
            Move(movingDirections);

            if (inputController.isDigging)
            {
                if (inputController.isLookingUp)
                {
                    animator.SetInteger("AnimState", 7);
                } else if (inputController.isLookingDown)
                {
                    animator.SetInteger("AnimState", 8);
                }
                else
                {
                    animator.SetInteger("AnimState", 6);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!string.IsNullOrEmpty(costume))
        {
            string[] oldSpriteNameList = spriteRenderer.sprite.name.Split('_');
            Sprite newSprite = Array.Find(subSprites, item => item.name.Split('_')[item.name.Split('_').Length - 1] == oldSpriteNameList[oldSpriteNameList.Length - 1]);

            if (newSprite)
            {
                spriteRenderer.sprite = newSprite;
            }
        }
    }

    protected void LoadSubSprites()
    {
        subSprites = Resources.LoadAll<Sprite>("Mole/" + costume);
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
            animator.SetInteger("AnimState", 4);

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
            if (isHittingChest && (isHittingLeft || isHittingRight) && chestClass)
            {
                chestClass.TryOpenChest();
            }
        }

        // destroy selected tiles
        if (grid)
        {
            foreach (Vector3 position in tilesToDestroy) {
                var positionCell = grid.WorldToCell(position);
                digableMap.SetTile(positionCell, null);
            }
        }
    }

    new protected void Move(List<string> directions)
    {
        if (directions.Contains("left") || directions.Contains("right"))
        {
            animator.SetInteger("AnimState", 1);
        }

        base.Move(directions);
    }

    protected IEnumerator MoveToXCoroutine(float targetX)
    {
        while (Mathf.Abs(transform.position.x - targetX) > 0.005 * Screen.width)
        {
            if (transform.position.x > targetX)
            {
                movingControlled = "left";
            }
            else
            {
                movingControlled = "right";
            }

            yield return new WaitForEndOfFrame();
        }

        movingControlled = null;
    }

    public void ChangeCostumeWithoutSelect(string costumeName)
    {
        string currentCostume = loadGame.safeData.currentCostume;
        if (costumeName != currentCostume || !(costumeName == "mole_standard" && currentCostume == null))
        {
            ChangeCostume(costumeName);
        }
    }

    public void ChangeCostumeWithSelect(string costumeName)
    {

        ChangeCostume(costumeName, false, true);
    }

    private void ChangeCostume(string costumeName, bool goToRight = true, bool saveCostume = false)
    {
        bool shouldChange = false;

        if (costumeName == "mole_standard" && costume == null)
        {
            return;
        }
        else if (costumeName != costume)
        {
            GameObject curtain = GameObject.Find("Curtain");
            float curtainX = curtain.transform.position.x;

            if (goToRight)
            {
                StartCoroutine(ChangeCostumeCoroutine(costumeName, curtainX, curtainX + 74));
            }
            else
            {
                StartCoroutine(ChangeCostumeCoroutine(costumeName, curtainX, curtainX - 100));
            }

            if (saveCostume)
            {
                loadGame.safeData.SetCurrentCostume(costumeName);
                loadGame.Save(loadGame.safeData);
                costumeAssortment.RefreshPurchasedCostumes();
            }
        }
    }

    protected IEnumerator ChangeCostumeCoroutine(string costumeName, float walkToPosition1, float walkToPosition2)
    {
        animator.SetInteger("AnimState", 1);

        StartCoroutine(MoveToXCoroutine(walkToPosition1));

        while (movingControlled != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(MoveToXCoroutine(walkToPosition2));

        this.costume = costumeName;
        LoadSubSprites();
    }

    /*
     * check collision triggerobjects (for collect coins, and finish the level)
     */
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("coin")) {
            Diamond diamond = other.gameObject.GetComponent<Diamond>();
            if (diamond.blue)
            {
                gameLogic.AddBlueDiamonds();
            }
            else
            {
                if(diamond.diamondShow == 0)
                {
                    loadGame.currentLevelData.diamondShow[0] = true;
                }
                else if(diamond.diamondShow == 1){
                    loadGame.currentLevelData.diamondShow[1] = true;
                }
                else if (diamond.diamondShow == 2)
                {
                    loadGame.currentLevelData.diamondShow[2] = true;
                }
                else if (diamond.diamondShow == 3)
                {
                    loadGame.currentLevelData.diamondShow[3] = true;
                }
                else if (diamond.diamondShow == 4)
                {
                    loadGame.currentLevelData.diamondShow[4] = true;
                }
            }

            Destroy(other.gameObject);
        }

        if (other.gameObject.name == "LevelEnd") {
            if (!hasFinished) {
                Respawn();
            }
        }

        if (other.gameObject.CompareTag("Finish")) {
            hasFinished = true;
            loadGame.safeData.diamonds += gameLogic.GetBlueDiamonds();
            if(loadGame.currentLevelData.diamondShow[0]){
                loadGame.safeData.diamonds += 2;
            }
            if (loadGame.currentLevelData.diamondShow[1])
            {
                loadGame.safeData.diamonds += 5;
            }
            if (loadGame.currentLevelData.diamondShow[2])
            {
                loadGame.safeData.diamonds += 10;
            }
            if (loadGame.currentLevelData.diamondShow[3])
            {
                loadGame.safeData.diamonds += 25;
            }
            loadGame.SaveLevel(loadGame.currentLevelData);
            loadGame.Save(loadGame.safeData);
            gameLogic.ChooseScene(0);
        }

        if (other.gameObject.name == "red_potion")
        {
            redPotionActive = true;

        }
    }

    /*
     * collisison with chestobject
     */
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Chest")
        {
            isHittingChest = true;
        }
    }
}