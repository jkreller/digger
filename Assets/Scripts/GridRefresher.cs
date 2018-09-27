using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridRefresher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tilemap in tilemaps) {
            tilemap.RefreshAllTiles();
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
