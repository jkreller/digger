using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameLogic gameLogic;
    private InputController inputController;
    private GameObject pauseMenu;
    private Button backToMenuButton;

    /*
     * Initializing menue
     */
    private void Start()
    {
        gameLogic = this.gameObject.GetComponent<GameLogic>();
        inputController = this.gameObject.GetComponent<InputController>();
        pauseMenu = GameObject.Find("PauseMenuCanvas");
        pauseMenu.SetActive(false);
    }

    /*
     * Get pausemenu on button
     */
    void Update()
    {
        if (inputController.pause)
        {
            TogglePause();
        }
    }

    /*
     * Set timescale on pausemenu
     */
    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }

    public void ChooseScene(int sceneIndex)
    {
        TogglePause();
        SceneManager.LoadScene(sceneIndex);
    }
}