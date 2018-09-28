using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
    public RawImage fadeImage;
    public float fadingSpeed = 0.5f;

    private Text diamondCount;
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
        if (countObject) {
            diamondCount = countObject.GetComponent<Text>();
        }

        updateDiamondText();

        fadeCount = fadeImage.color.a;
        fadeImage.gameObject.SetActive(false);

        StartCoroutine(Fade("Out", false));
    }

    /*
     * Update is called once per frame
     */
    void Update()
    {
        if (diamondCount)
        {
            updateDiamondText();
        }
    }

    /*
     * Add diamonds to count
     */
    public void addDiamonds(int count){
        diamonds += count;
    }

    /*
     * Switches to next scene with fading
     */
    public void nextScene() {
        StartCoroutine(Fade("In"));
        StartCoroutine(coroutineNextScene());
    }

    /*
     * Switches to first scene with fading
     */
    public void restartGame()
    {
        StartCoroutine(Fade("In"));
        StartCoroutine(coroutineNextScene(true));
    }

    /*
     * Updates diamond count text in view
     */
    private void updateDiamondText() {
        if (diamondCount) {
            diamondCount.text = diamonds.ToString();
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
    private IEnumerator coroutineNextScene(bool toStartingScene = false) {
        while (!finishedFading)
            yield return null;

        finishedFading = false;
		
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation asyncLoad;
        if (!toStartingScene) {
            asyncLoad = SceneManager.LoadSceneAsync(actualSceneIndex + 1);
        } else {
            asyncLoad = SceneManager.LoadSceneAsync(0);
        }

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}