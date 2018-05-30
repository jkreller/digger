using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackground : MonoBehaviour {
    public int textureSize = 32;
    public bool scaleHorizontally = true;
    public bool scaleVertically = true;

	// Use this for initialization
    void Start () {
        var newWidth = 1f;
        var newHeight = 1f;

        if (scaleHorizontally) {
            newWidth = Mathf.Ceil(Screen.width / (textureSize * PixelPerfectCamera.scale));
        }

        if (scaleVertically) {
            newHeight = Mathf.Ceil(Screen.height / (textureSize * PixelPerfectCamera.scale));
        }

        transform.localScale = new Vector3(newWidth * textureSize, newHeight * textureSize, 1);

        GetComponent<Renderer>().material.mainTextureScale = new Vector3(newWidth, newHeight, 1);
	}
}
