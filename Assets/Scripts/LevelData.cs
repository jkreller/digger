using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelData
{
    public static LevelData current;
    public int blueTotal;
    public int blue;
    public bool green;
    public bool red;
    public bool yellow;
    public bool black;
    public bool gold;
    public bool finished;

    public LevelData(){
        
    }
}
