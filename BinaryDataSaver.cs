using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Simple binary data saver. Used for global game data which needs to be persistant.
/// Based on https://unity3d.com/learn/tutorials/topics/scripting/introduction-saving-and-loading
/// Uses public static methods so that it can be accessed from anywhere.
/// </summary>
public class BinaryDataSaver : MonoBehaviour
{
    const string folderName = "BinarySaveData";
    const string fileExtension = ".dat";

    public static void SaveData(SaveData data)
    {
        //Path
        Debug.Log("trying to save data");
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string dataPath = Path.Combine(folderPath, data.dataName + fileExtension);


        //Actual saving
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, data);
        }
    }

    public static SaveData LoadData(SaveData data)
    {
        //Find the path based on the data name
        Debug.Log("trying to load data");

        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string dataPath = Path.Combine(folderPath, data.dataName + fileExtension);


        //Actual loading
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (File.Exists(dataPath))
        {
            using (FileStream fileStream = File.Open(dataPath, FileMode.Open))
            {
                return (SaveData)binaryFormatter.Deserialize(fileStream);
            }
        }
        else return null;
    }
}