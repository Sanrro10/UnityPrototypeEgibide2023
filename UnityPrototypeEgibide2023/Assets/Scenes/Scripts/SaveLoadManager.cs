using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    
    private static string nameFile = "savegame";
    private static string extensionFile = ".json";
    
    public static void SaveGame(GameData gameData, string file)
    {
        string json = JsonUtility.ToJson(gameData);
        string path = Application.persistentDataPath + "/"+ file;
        File.WriteAllText(path, json);
        
        /*Debug.Log("SaveLoadManager -> GameData.position: " + gameData.spawnPosition);
        Debug.Log("SaveLoadManager -> GameData.scene: " + gameData.spawnScene);
        Debug.Log("SaveLoadManager -> json: " + json);
        Debug.Log("SaveLoadManager -> path: " + path);*/
    }

    public static GameData LoadGame(string slotForFile)
    {
        string path = Application.persistentDataPath + "/" + nameFile + slotForFile + extensionFile ;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            try {
                GameData gameData = JsonUtility.FromJson<GameData>(json);
                gameData.isValid = true;
                return gameData;
            } catch {
                Debug.LogWarning("Horrible things happened! - NullReferenceException: Object reference not set to an instance of an object. The object does not exist in the file: " + nameFile + slotForFile + extensionFile);
                return returnIsValidFalse();
            }
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return returnIsValidFalse();
        }
    }
    
    /*public static void CreateGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        string path = Application.persistentDataPath + "/savegame.json";
        File.WriteAllText(path, json);
    }*/

    private static GameData returnIsValidFalse()
    {
        GameData gameData = new GameData();
        gameData.isValid = false;
        return gameData;
    }
    
}
