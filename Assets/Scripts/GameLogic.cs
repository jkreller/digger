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

        StartCoroutine(Fade("Out"));
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
    private IEnumerator Fade(string direction, bool actionAfter = true) {
        var speed = Time.deltaTime / fadingSpeed;

        fadeImage.gameObject.SetActive(true);

        switch (direction) {
            case "In":
                while (fadeCount < 1f)
                {
                    fadeCount += speed;
                    fadeImage.color = new Color(0, 0, 0, fadeCount);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;
            case "Out":
                while (fadeCount > 0f)
                {
                    fadeCount -= speed;
                    fadeImage.color = new Color(0, 0, 0, fadeCount);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;
        }

        if (actionAfter) {
            finishedFading = true;
            fadeImage.gameObject.SetActive(false);
        }
    }

    /*
     * Coroutine for switching to next scene, i.e. after fading
     * If toStartingScene is true the first scene is loaded
     */
    private IEnumerator coroutineNextScene(bool toStartingScene = false) {
        while (!finishedFading)
            yield return new WaitForSeconds(0.1f);

        finishedFading = false;

        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (!toStartingScene) {
            SceneManager.LoadScene(actualSceneIndex + 1);
        } else {
            SceneManager.LoadScene(0);
        }
    }
}