using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridRefresher : MonoBehaviour
{
    /*
     * refreshing tiles before startup
     */
    void Start()
    {
        var tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tilemap in tilemaps) {
            tilemap.RefreshAllTiles();
        }
    }
}
