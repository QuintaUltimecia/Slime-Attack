using System;
using UnityEngine;

public static class DateSaver
{
    private static string _year = "year";
    private static string _month = "month";
    private static string _day = "day";

    public static void SaveDate()
    {
        if (PlayerPrefs.GetInt(_year) == 0)
        {
            DateTime date = DateTime.Now;

            PlayerPrefs.SetInt(_year, date.Year);
            PlayerPrefs.SetInt(_month, date.Month);
            PlayerPrefs.SetInt(_day, date.Day);
        }
    }

    public static DateTime LoadDate()
    {
        return new DateTime(
            year: PlayerPrefs.GetInt(_year),
            month: PlayerPrefs.GetInt(_month),
            day: PlayerPrefs.GetInt(_day));
    }
}
