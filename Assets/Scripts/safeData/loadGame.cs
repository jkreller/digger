using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class loadGame : MonoBehaviour
{
    public int id;
    public static SafeData safeData = new SafeData();
    public static LevelData currentLevelData;

    void Awake()
    {
        Load();
        GetActualLevel(id);
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveGame.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.gd", FileMode.Open);
            safeData = (SafeData)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void Save(SafeData safeData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        File.Delete(Application.persistentDataPath + "/saveGame.gd");
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.gd");
        bf.Serialize(file, safeData);
        file.Close();
    }

    public static void SaveLevel(LevelData levelData){
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

    public void GetActualLevel(int id){
        currentLevelData = safeData.levels.Find(x => x.id == id);
    }

    public void DeleteSafeData()
    {
        File.Delete(Application.persistentDataPath + "/saveGame.gd");
    }
}
