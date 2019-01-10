using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
    public RawImage fadeImage;
    public float fadingSpeed = 0.5f;
    private int blueDiamonds;
    private int blueTotal;
    private Text blueCount;
    private Text blueSumCount;
    private int diamonds;
    private float fadeCount;
    private bool finishedFading;
    private int timesFading;

    /*
     * Use this for initialization
     */
    void Start()
    {
        GameObject countObject = GameObject.Find("DiamondCount");
        GameObject sumObject = GameObject.Find("DiamondSum");

        if (countObject) {
            blueCount = countObject.GetComponent<Text>();
        }
        else
        {
            blueSumCount = sumObject.GetComponent<Text>();
        }

        if (loadGame.currentLevelData != null)
        {
            blueTotal = loadGame.currentLevelData.blueTotal;
        }
        UpdateDiamondText();

        fadeCount = fadeImage.color.a;
        fadeImage.gameObject.SetActive(false);

        StartCoroutine(Fade("Out", false));
    }

 

    /*
     * Add diamonds to count
     */
    public void AddBlueDiamonds(){
        blueDiamonds++;
        UpdateDiamondText();
        if(blueDiamonds > loadGame.currentLevelData.blue)
        {
            loadGame.currentLevelData.blue = blueDiamonds; 
        }

    }

    public int GetBlueDiamonds(){
        return blueDiamonds;
    }

    /*
     * Switches to next scene with fading
     */
    public void NextScene() {
        StartCoroutine(Fade("In"));
        StartCoroutine(CoroutineNextScene());
    }

    public void ChooseScene(int sceneIndex) {
        StartCoroutine(Fade("In"));
        StartCoroutine(CoroutineNextScene(sceneIndex));
    }

    /*
     * Switches to first scene with fading
     */
    public void RestartGame()
    {
        StartCoroutine(Fade("In"));
        StartCoroutine(CoroutineNextScene(0));
    }

    /*
     * Updates diamond count text in view
     */
    public void UpdateDiamondText() {
        if (blueCount) {
            blueCount.text = blueDiamonds.ToString()+"/"+blueTotal.ToString();
        }
        else if (blueSumCount)
        {
            blueSumCount.text = loadGame.safeData.diamonds.ToString();
        }
    }

    /*
     * Coroutine for fading black in or out
     * 
     * Fading depends on alpha channel of fadingImage
     */
    public IEnumerator Fade(string direction, bool actionAfter = true) {
        var speed = Time.deltaTime / fadingSpeed;

        fadeImage.gameObject.SetActive(true);

        switch (direction) {
            case "In":
                while (fadeCount < 1f)
                {
                    fadeCount += speed;
                    fadeImage.color = new Color(0, 0, 0, fadeCount);
                    yield return new WaitForEndOfFrame();
                }
                break;
            case "Out":
                while (fadeCount > 0f)
                {
                    fadeCount -= speed;
                    fadeImage.color = new Color(0, 0, 0, fadeCount);
                    yield return new WaitForEndOfFrame();
                }
                break;
        }

        if (timesFading == 0) {
            fadeImage.gameObject.SetActive(false);
        }

        if (actionAfter) {
            finishedFading = true;
        }

        timesFading++;
    }

    /*
     * Coroutine for switching to next scene, i.e. after fading
     * If toStartingScene is true the first scene is loaded
     */
    private IEnumerator CoroutineNextScene(int sceneIndex = -1) {
        while (!finishedFading)
            yield return null;

        finishedFading = false;
        int actualSceneIndex;

        if (sceneIndex >= 0) {
            actualSceneIndex = sceneIndex;
        }
        else{
            actualSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        }



        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(actualSceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}