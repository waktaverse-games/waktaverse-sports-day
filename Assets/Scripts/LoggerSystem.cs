using System;
using System.IO;
using System.Text;
using UnityEngine;

public class LoggerSystem
{
    private static readonly string LogFilePath;
    private static readonly FileInfo LogFileInfo;

    static LoggerSystem()
    {
        var time = GetTimeStamp(false);
        LogFilePath = $"{Application.persistentDataPath}/Logs/{time}_Test.log.csv";
        LogFileInfo = new FileInfo(LogFilePath);
        
        if (!Directory.Exists(LogFileInfo.DirectoryName))
        {
            Debug.Log("Creating log directory");
            Directory.CreateDirectory(LogFileInfo.DirectoryName!);
        }
    }
    
    public static void Log(params string[] args)
    {
        if (!Directory.Exists(LogFileInfo.DirectoryName))
        {
            Debug.Log("Creating log directory");
            Directory.CreateDirectory(LogFileInfo.DirectoryName!);
        }
        
        if (!File.Exists(LogFilePath))
        {
            CreateLogFile();
        }
        
        Debug.Log("Logging to file");
        using (var sw = File.AppendText(LogFilePath))
        {
            var time = GetTimeStamp();
            var nickname = GameDatabase.NickName.Replace(',', '_').Trim();
            sw.Write(time);
            sw.Write(",");
            sw.Write(nickname);
            sw.Write(",");
            sw.Write(string.Join(",", args));
            sw.Write(",\n");
        }
    }
    
    private static void CreateLogFile()
    {
        using (var sw = File.CreateText(LogFilePath))
        {
            var labels = new string[] { "Time", "Nickname", "Mode", "Game", "Score" };
            sw.Write(string.Join(",", labels));
            sw.Write(",\n");
        }
    }
    
    private static string GetTimeStamp(bool includeTime = true)
    {
        return DateTime.Now.ToString(includeTime ? "MM-dd HH:mm" : "MM-dd");
    }
}