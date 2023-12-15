using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);

        string path = Application.persistentDataPath + "/savegame.json";
        File.WriteAllText(path, json);
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            return gameData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
