using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public static LevelData current;
    public int id;
    public int blueTotal;
    public int blue;
    public bool[] diamondShow = new bool[5];
    public bool finished;

    public LevelData(){
        
    }
}
