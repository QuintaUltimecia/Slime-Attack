using Facebook.Unity;
using System;
using UnityEngine;

public static class Analytics
{
    private static float _timeLevel;

    public static void Start()
    {
        string key = "start_game";
        int valueStart = PlayerPrefs.GetInt(key);
        valueStart++;
        PlayerPrefs.SetInt(key, valueStart);

        string json = '{' + $" \"count\": \"{valueStart}\", \"days since reg\": \"{GetDays()}\"" + '}';
        AppMetrica.Instance.ReportEvent(key, json);

        FB.LogAppEvent(key, valueStart);
    }

    public static void StartGame()
    {
        string key = "level_start";
        int levelStart = PlayerPrefs.GetInt(key);
        levelStart++;
        PlayerPrefs.SetInt(key, levelStart);

        AppMetrica.Instance.SendEventsBuffer();
        string json = '{' + $" \"count\": \"{levelStart}\", \"days since reg\": \"{GetDays()}\" " + '}';
        AppMetrica.Instance.ReportEvent(key, json);

        FB.LogAppEvent(key, levelStart);

        DateTime time = DateTime.Now;
        _timeLevel = time.Second;
    }

    public static void GameOver()
    {
        DateTime time = DateTime.Now;
        _timeLevel = time.Second - _timeLevel;

        string key = "level_start";
        int levelStart = PlayerPrefs.GetInt(key);

        AppMetrica.Instance.SendEventsBuffer();
        string json = '{' + $" \"count\": \"{levelStart}\", \"time_spent\": \"{_timeLevel}\", \"days since reg\": \"{GetDays()}\" " + '}';
        AppMetrica.Instance.ReportEvent("level_complete", json);

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
