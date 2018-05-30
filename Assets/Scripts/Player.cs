using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 150f;
    public Vector2 maxVelocity = new Vector2(60, 100);

    private Rigidbody2D body2D;
    private SpriteRenderer renderer2D;

	// Use this for initialization
	void Start () {
        body2D = GetComponent<Rigidbody2D>();
        renderer2D = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        var absValX = Mathf.Abs(body2D.elocity.x);
	}
}
