using System.IO;
using Newtonsoft.Json;

public static class JsonUtil
{
    public static T LoadData<T>(string path) where T : class, new()
    {
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            return null;
        }
        
        var loadedData = File.ReadAllText(path);
        var data = JsonConvert.DeserializeObject<T>(loadedData);

        return data;
    }
    
    public static void SaveData<T>(T data, string path)
    {
        var result = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, result);
    }
}