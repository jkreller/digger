using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class loadGame : MonoBehaviour
{

    public static SafeData safeData = new SafeData();
 
    void Awake()
    {
        Load();
    }




    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveGame.gd"))
        {
            Debug.Log("load game");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.gd", FileMode.Open);
            safeData = (SafeData)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void Save(SafeData safeData)
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.gd");
        bf.Serialize(file, safeData);
        file.Close();
    }
}
