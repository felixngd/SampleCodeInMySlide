using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad
{
    public static void Save<T>(T data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static T Load<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            T data = (T)formatter.Deserialize(fileStream);
            fileStream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return default(T);
        }
    }
}
