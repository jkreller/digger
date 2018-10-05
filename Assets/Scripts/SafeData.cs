using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SafeData
{
    public static SafeData current;
    public LevelData level1;
    public LevelData level2;


    public SafeData()
        {
            level1 = new LevelData();
            level2 = new LevelData();
           
        }
}