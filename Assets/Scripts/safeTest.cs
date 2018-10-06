using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeTest : MonoBehaviour
{
    void Start()
    {
        SafeData testData = new SafeData();
        LevelData grassland = new LevelData();
        grassland.black = true;
        grassland.blueTotal = 40;
        List<LevelData> levellist = new List<LevelData>();
        levellist.Add(grassland);
        testData.levels = levellist;
        loadGame.Save(testData);
        loadGame.Load();
        Debug.Log(loadGame.safeData.levels);

    }

    void Update()
    {
        loadGame.Load();
        Debug.Log(loadGame.safeData.levels);
    }
}
