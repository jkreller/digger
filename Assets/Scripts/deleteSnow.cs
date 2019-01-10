using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteSnow : MonoBehaviour
{
    protected Camera camera;
    public Sprite[] pics;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        SpriteRenderer spriteRenderer;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pics[Random.Range(0, pics.Length)];

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y <= (camera.transform.position.y-camera.orthographicSize-200)){

            Destroy(this.gameObject);
        }
    }
}
