using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Achievement[] achievements;
    public Achievement[] levelAchievements;

    void OnEnable()
    {
        Achievements.All.Clear();
        Achievements.LevelAchievements.Clear();

        Achievements.All.AddRange(levelAchievements);
        Achievements.All.AddRange(achievements);
        Achievements.LevelAchievements.AddRange(levelAchievements);

        Achievements.LoadAchievements();

        LoadAchievements();
    }

    public void LoadAchievements()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        List<Achievement> completed = Achievements.Completed;
        foreach(Achievement achievement in completed)
        {
            GameObject go = Instantiate(achievementPrefab, transform);

            go.GetComponent<AchievementItem>().achievement = achievement;
            go.GetComponent<AchievementItem>().Load();
        }

        GetComponent<RectTransform>().sizeDelta.Set(0, 64 * completed.Count);
    }

    public void CompleteAchievement(int id)
    {
        Achievements.CompleteAchievement(id);
    }

    public void ResetAchievements()
    {
        Achievements.ResetAchievements();
    }
}

public static class Achievements
{
    public static Action<int> onAchievementUnlocked = null;

    public static List<Achievement> All = new List<Achievement>();
    public static List<Achievement> Completed = new List<Achievement>();
    public static List<Achievement> LevelAchievements = new List<Achievement>();

    public static Dictionary<int, Achievement> MappedToID = new Dictionary<int, Achievement>();

    public const string ACHIEVEMENTS_KEY = "twr.encodedAchievements";

    public static void LoadAchievements()
    {
        MappedToID.Clear();

        foreach (Achievement achievement in All) MappedToID.Add(achievement.id, achievement);

        if (PlayerPrefs.GetString(ACHIEVEMENTS_KEY) != "")
        {
            Completed.Clear();

            string savedString = PlayerPrefs.GetString(ACHIEVEMENTS_KEY);

            string[] stringIds = savedString.Split(' ');
            int[] ids = new int[stringIds.Length];

            for (int i = 0; i < stringIds.Length; i++) 
                ids[i] = int.Parse(stringIds[i]);

            foreach(int id in ids)
                Completed.Add(MappedToID[id]);
        }
    }

    public static void SaveAchievements()
    {
        string saveString = "";

        foreach(Achievement achievement in Completed)
        {
            saveString += achievement.id + " ";
        }

        PlayerPrefs.SetString(ACHIEVEMENTS_KEY, saveString.Trim());
    }

    public static void CompleteAchievement(int id)
    {
        if (!Completed.Contains(MappedToID[id]) && GameSaves.currentSave != 4)
        {
            Completed.Add(MappedToID[id]);
            onAchievementUnlocked.Invoke(id);
            SaveAchievements();
        }
    }

    public static void ResetAchievements()
    {
        Completed.Clear();
        SaveAchievements();
    }
}

[Serializable]
public class Achievement
{
    public string name;
    public string description;
    public Sprite image;
    public int id;
}
