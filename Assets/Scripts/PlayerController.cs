using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Vector2 moving = new Vector2();

	// Update is called once per frame
	void Update () {
        moving.x = moving.y = 0;

        if (Input.GetKey("right")) {
            moving.x = 1;
        } else if (Input.GetKey("left")) {
            moving.x = -1;
        }

        if (Input.GetKey("space")) {
            moving.y = 1;
        }
	}
}