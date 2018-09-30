using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PauseMenu : MonoBehaviour
{
    private GameLogic gameLogic;
    private InputController inputController;
    private Canvas pauseMenu;

    private void Start()
    {
        gameLogic = this.gameObject.GetComponent<GameLogic>();
        inputController = this.gameObject.GetComponent<InputController>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<Canvas>();
    }

    void Update()
    {
        if (inputController.pause)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pauseMenu.enabled = false;
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.enabled = true;
        }
    }
}