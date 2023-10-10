using System.IO;
using UnityEngine;

public class SaveSystem
{
    private string _fileName = "SaveData.json";
    private string _editorDirectory = "_Slime Attack io/Saves";
    private string _path;

    private bool _isEncryption = false;

    public SaveSystem(bool isEncryption)
    {
        _isEncryption = isEncryption;

#if UNITY_ANDROID && !UNITY_EDITOR
        _path = $"{Application.persistentDataPath}{Path.AltDirectorySeparatorChar}/{_fileName}";
#else
        _path = $"{Application.dataPath}/{_editorDirectory}/{_fileName}";
#endif
    }

    public void Save(BaseData baseData)
    {
        string json = JsonUtility.ToJson(baseData);

        if (_isEncryption == true)
        {
            try
            {
                var clientBytes = System.Text.Encoding.UTF8.GetBytes(json);
                json = System.Convert.ToBase64String(clientBytes);
            }
            catch
            {
                Debug.LogWarning("Current File is not a Base64");
            }
        }

        using (var writer = new StreamWriter(_path))
            writer.WriteLine(json);
    }

    public T Load<T>() where T : BaseData
    {
        string json = "";

        if (!File.Exists(_path))
            return null;

        using (var reader = new StreamReader(_path))
            json += reader.ReadLine();

        if (_isEncryption == true)
        {
            try
            {
                byte[] buffer = System.Convert.FromBase64String(json);
                json = System.Text.Encoding.ASCII.GetString(buffer);
            }
            catch 
            {
                Debug.LogWarning("Current File is not a Base64");
            }
        }

        return JsonUtility.FromJson<T>(json);
    }
}
