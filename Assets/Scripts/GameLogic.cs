using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
    private Text diamondCount;
    private int diamonds;

    void Start()
    {
        GameObject countObject = GameObject.Find("diamondCount");
        diamondCount = countObject.GetComponent<Text>();

        updateDiamondText();
    }

    void Update()
    {
        updateDiamondText();
    }

    public void addDiamonds(int count){
        diamonds += count;
    }

    public void nextScene() {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(actualSceneIndex + 1);
    }

    private void updateDiamondText() {
        if (diamondCount) {
            diamondCount.text = diamonds.ToString();
        }
    }
}