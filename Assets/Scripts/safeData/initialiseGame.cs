using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class initialiseGame : MonoBehaviour
{
    public List <LevelData> levelData;

    void Awake()
    {
        //File.Delete(Application.persistentDataPath + "/saveGame.gd");

        if (!File.Exists(Application.persistentDataPath + "/saveGame.gd"))
        {
            SafeData testData = new SafeData();
            testData.levels = levelData;
            loadGame.Save(testData);

        }

      
    }

	private void Start()
	{
        
        loadGame.Load();
        GameObject countObject = GameObject.Find("DiamondCounter");
        Text blueCount = countObject.GetComponent<Text>();
        blueCount.text = loadGame.safeData.diamonds.ToString();
	}
}

