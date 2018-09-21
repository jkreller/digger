using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    public float scrollSpeed = 0.1f;
    public float parallaxScrollSpeed = 6;
    public bool moveWithPlayer = true;

    protected MolePlayer molePlayer;
    protected Camera mainCamera;
    protected Renderer rendererInstance;

	// Use this for initialization
	void Start () {
        rendererInstance = GetComponent<Renderer>();
        molePlayer = FindObjectOfType<MolePlayer>();
        mainCamera = FindObjectOfType<Camera>();

        var cameraPos = mainCamera.transform.position;
        cameraPos.z = transform.position.z;
        transform.position = cameraPos;

        SetSize();
	}
	
	// Update is called once per frame
	void Update () {
        // scroll background sidewards
        var x = Mathf.Repeat(Time.time * scrollSpeed, 1);

        var parallaxOffsetY = 0f;

        if (molePlayer) {
            if (moveWithPlayer) {
                // keep staying with player
                var playerPos = molePlayer.transform.position;
                playerPos.z = transform.position.z;
                transform.position = playerPos;
            }

            // scrolling a bit when player jumps
            var playerVelocity = molePlayer.GetComponent<Rigidbody2D>().velocity;
            parallaxOffsetY = moveWithPlayer ? playerVelocity.y * parallaxScrollSpeed / 10000 : 0;
        }

        var offset = new Vector2(x, parallaxOffsetY);

        rendererInstance.material.mainTextureOffset = offset;
	}

    void SetSize() {
        // set size for fitting perfectly into camera
        var oldSize = transform.localScale;
        var newSize = oldSize;

        // calculate new height
        newSize.y = mainCamera.orthographicSize * 2;
        // add padding
        newSize.y += newSize.y / 10;

        // calculate new width
        newSize.x = mainCamera.aspect * newSize.y;
        // add padding
        newSize.x += newSize.x / 10;

        // set new size
        transform.localScale = newSize;

        // calculate and set new tiling for x axis
        var newTilingX = newSize.x / newSize.y;
        rendererInstance.material.mainTextureScale = new Vector2(newTilingX, 1);
    }
}