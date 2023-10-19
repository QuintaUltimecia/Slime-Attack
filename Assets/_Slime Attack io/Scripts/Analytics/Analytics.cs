using Facebook.Unity;
using System;
using UnityEngine;

public static class Analytics
{
    private static float _timeLevel;

    public static void Start()
    {
        string key = "startgame";
        int valueStart = PlayerPrefs.GetInt(key);
        valueStart++;
        PlayerPrefs.SetInt(key, valueStart);

        string json = '{' + $" \"count\": \"{valueStart}\", \"days since reg\": \"{GetDays()}\"" + '}';
        //AppMetrica.Instance.ReportEvent("game_start", json);

        FB.LogAppEvent("game_start", valueStart);
    }

    public static void StartGame()
    {
        string key = "levelstart";
        int levelStart = PlayerPrefs.GetInt(key);
        levelStart++;
        PlayerPrefs.SetInt(key, levelStart);

        //AppMetrica.Instance.SendEventsBuffer();
        //string json = '{' + $" \"level\": \"{_canvas.GamePanel.LevelManager.Count}\", \"days since reg\": \"{GetDays()}\" " + '}';
        //AppMetrica.Instance.ReportEvent("level_start", json);

        FB.LogAppEvent("level_start", levelStart);

        DateTime time = DateTime.Now;
        _timeLevel = time.Second;
    }

    public static void GameOver()
    {
        DateTime time = DateTime.Now;
        _timeLevel = time.Second - _timeLevel;

        //AppMetrica.Instance.SendEventsBuffer();
        //string json = '{' + $" \"level\": \"{_canvas.GamePanel.LevelManager.Count}\", \"time_spent\": \"{_timeLevel}\", \"days since reg\": \"{GetDays()}\" " + '}';
        //AppMetrica.Instance.ReportEvent("level_complete", json);

        string key = "levelstart";
        int levelStart = PlayerPrefs.GetInt(key);

        FB.LogAppEvent("level_complete", levelStart);
    }

    private static int GetDays()
    {
        DateSaver.SaveDate();
        DateTime dateTime = DateSaver.LoadDate();
        DateTime dateTimeNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        return (dateTimeNow - dateTime).Days;
    }
}
