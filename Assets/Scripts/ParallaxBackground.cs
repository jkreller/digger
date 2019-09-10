using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("References")]
    private Camera mainCamera;
    [SerializeField] private Transform referenceLayer;
    private MolePlayer molePlayer;

    [Header("Options")]
    [SerializeField] private float parallaxSpeed = 0.25f;
    [SerializeField] private float backgroundSize = 10;
    [SerializeField] private bool placeAtStart = true;
    [SerializeField] private float overflow;

    [Header("Logic fields")]
    private Transform[] layers;
    private float lastCameraX;
    private float lastCameraY;
    private int leftIndex;
    private int rightIndex;

    void Start()
    {
        // Set references
        mainCamera = Camera.main;
        molePlayer = FindObjectOfType<MolePlayer>();

        int childCount = transform.childCount;
        layers = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            layers[i] = transform.GetChild(i);
            if (placeAtStart)
            {
                // Position layers in root element
                layers[i].transform.position = new Vector3(transform.position.x, transform.position.y, layers[i].transform.position.z);
            }
        }

        if (placeAtStart)
        {
            // Position root element according to camera
            PositionAndSize();
        }

        // Set indeces
        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void Update()
    {
        // Move y-axis with camera
        transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);

        // Parallax effect
        float dX = mainCamera.transform.position.x - lastCameraX;
        float dY = mainCamera.transform.position.y - lastCameraY;
        transform.position += Vector3.right * dX * parallaxSpeed;
        transform.position += Vector3.up * dY * parallaxSpeed;
        lastCameraX = mainCamera.transform.position.x;
        lastCameraY = mainCamera.transform.position.y;
    }

    private void ScrollLeft()
    {
        float x = layers[leftIndex].position.x - backgroundSize;
        layers[rightIndex].position = new Vector3(x, layers[rightIndex].position.y, layers[rightIndex].position.z);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        float x = layers[leftIndex].position.x + backgroundSize;
        layers[leftIndex].position = new Vector3(x, layers[leftIndex].position.y, layers[leftIndex].position.z);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    private void PositionAndSize()
    {
        // Position root element according to camera
        Vector3 cameraPos = mainCamera.transform.position;
        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);

        // Scale height of root element according to camera
        SpriteRenderer spriteRenderer = referenceLayer.GetComponent<SpriteRenderer>();

        Vector3 newScale = Vector3.one;

        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;

        newScale.x = (float)worldScreenHeight / width + overflow;
        newScale.y = (float)worldScreenHeight / height + overflow;

        transform.localScale = newScale;
    }
}