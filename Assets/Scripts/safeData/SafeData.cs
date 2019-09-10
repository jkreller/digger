using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SafeData
{
    public static SafeData current;
    public List<LevelData> levels;
    public int diamonds;
    public int redDiamonds;
    public int greenDiamonds;
    public int yellowDiamonds;
    public int blackDiamonds;
    public int specialDiamonds;
    public string currentCostume;
    public List<string> purchasedCostumes = new List<string>();

    public void SetCurrentCostume(string costumeName)
    {
        if (purchasedCostumes.Contains(costumeName))
        {
            currentCostume = costumeName;
        } else if (costumeName == "mole_standard")
        {
            currentCostume = null;
        }
    }
}