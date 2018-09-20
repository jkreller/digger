using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
    
    private Text diamondCount;
    private int diamonds;



    void Start()
    {
        

        GameObject anzeige = GameObject.Find("diamondCount");
        diamondCount = anzeige.GetComponent<Text>();

        diamondCount.text = diamonds.ToString();
    }

    void Update()
    {
        diamondCount.text = diamonds.ToString();

    }

    public void addDiamonds(int anzahl){

        diamonds += anzahl;

    }
}