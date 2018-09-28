using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class pauseMenue : MonoBehaviour
    {
        bool paused = false;
        private GameLogic gameLogic;
        private InputController inputController;



    private void Start()
	{
        gameLogic = this.gameObject.GetComponent<GameLogic>();
        inputController = this.gameObject.GetComponent<InputController>();
       
	}
	void Update()
        {

            
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameLogic.StartCoroutine(gameLogic.Fade("In"));
                paused = togglePause();

                

            }
        }

        void OnGUI()
        {
            if (paused)
            {
                
            }
        }

        bool togglePause()
        {
            if (Time.timeScale == 0f)
            {
                inputController.pause = false;
                Time.timeScale = 1f;
                return (false);
            }
            else
            {
                inputController.pause = true;
                Time.timeScale = 0f;
                return (true);
            }
        }
    }

