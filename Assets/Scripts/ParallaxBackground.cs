using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("References")]
    private Camera mainCamera;

    [Header("Options")]
    [SerializeField] private float parallaxSpeedX = 1;
    [SerializeField] private float parallaxSpeedY = 0.2f;
    [SerializeField] private float backgroundSize = 10;
    [SerializeField] private float overflow;

    [Header("Logic fields")]
    private Transform[] layers;
    private float lastCameraX;
    private float lastCameraY;

    void Start()
    {
        // Set references
        mainCamera = Camera.main;

        int childCount = transform.childCount;
        layers = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        // Move y-axis with camera
        transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);

        // Parallax effect
        foreach (Transform child in transform)
        {
            float dX = mainCamera.transform.position.x - lastCameraX;
            float dY = mainCamera.transform.position.y - lastCameraY;
            child.transform.position += Vector3.right * dX * parallaxSpeedX * child.transform.position.z * 0.001f;
            child.transform.position -= Vector3.up * dY * parallaxSpeedY * child.transform.position.z * 0.001f;
        }
        lastCameraX = mainCamera.transform.position.x;
        lastCameraY = mainCamera.transform.position.y;
    }

    private void PositionAndSize()
    {
        // Position root element according to camera
        Vector3 cameraPos = mainCamera.transform.position;
        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);

        // Scale height of root element according to camera
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();

        Vector3 newScale = Vector3.one;

        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;

        newScale.x = (float)worldScreenHeight / width + overflow;
        newScale.y = (float)worldScreenHeight / height + overflow;

        transform.localScale = newScale;
    }
}