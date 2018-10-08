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
        Debug.Log(safeData.levels.ToString());
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
        Debug.Log("safe");
        BinaryFormatter bf = new BinaryFormatter();
        File.Delete(Application.persistentDataPath + "/saveGame.gd");
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.gd");
        bf.Serialize(file, safeData);
        file.Close();
    }

    public static void saveLevel(LevelData levelData){
        LevelData levelInSafeData = safeData.levels.Find(x => x.id == levelData.id);
        if(levelInSafeData != null){
            safeData.levels.Remove(levelInSafeData);
            safeData.levels.Add(levelData);
        }
        else{
            safeData.levels.Add(levelData);
        }
        Save(safeData);
    }
}
