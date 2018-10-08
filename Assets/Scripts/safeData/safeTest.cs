using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class safeTest : MonoBehaviour
{
    public List <LevelData> levelData;

    void Awake()
    {
        SafeData testData = new SafeData();
        testData.levels = levelData;
        loadGame.Save(testData);
        loadGame.Load();

    }
}
