using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class pauseMenue : MonoBehaviour
    {
        bool paused = false;
        private GameLogic gameLogic;
        private InputController inputController;
    private Canvas menue;



    private void Start()
	{
        gameLogic = this.gameObject.GetComponent<GameLogic>();
        inputController = this.gameObject.GetComponent<InputController>();
        menue = GameObject.Find("Menue").GetComponent<Canvas>();
       
	}
	void Update()
        {

            
        if (Input.GetKeyDown(KeyCode.Escape))
            {
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
                menue.enabled = false;
                return (false);
            }
            else
            {
            
                inputController.pause = true;
                Time.timeScale = 0f;
                menue.enabled = true;
                return (true);
                
                
            }
        }
    }

