using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndText : MonoBehaviour
{
    /*
     * class for endscreen
     */    
    private GameLogic gameLogic;
    void Start()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    void RestartGame()
    {
        gameLogic.restartGame();
    }
}