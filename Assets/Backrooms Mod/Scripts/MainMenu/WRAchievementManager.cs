using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UMod;
using UnityEngine;

namespace Wobblyrooms.MainMenu
{
    public class WRAchievementManager : ModScriptBehaviour
    {
        public static IModPersistentData PersistentData { get; private set; }

        public GameObject achievementPrefab;
        public Achievement[] achievements;
        public LevelAchievement[] levelAchievements;

        public override void OnModLoaded()
        {
            base.OnModLoaded();

            PersistentData = ModPersistentData;
        }

        public void OnEnable()
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
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            List<Achievement> completed = Achievements.Completed;
            foreach (Achievement achievement in completed)
            {
                GameObject go = Instantiate(achievementPrefab, transform);

                go.GetComponent<AchievementItem>().achievement = achievement;
                go.GetComponent<AchievementItem>().Load();
            }

            GetComponent<RectTransform>().sizeDelta = new Vector2(0, 64 * completed.Count);
        }

        public void CompleteAchievement(int id)
        {
            Achievements.CompleteAchievement(id);
        }

        public void ForceCompleteAchievement(int id)
        {
            Achievements.ForceCompleteAchievement(id);
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
        public static List<LevelAchievement> LevelAchievements = new List<LevelAchievement>();

        public static Dictionary<int, Achievement> MappedToID = new Dictionary<int, Achievement>();
        public static Dictionary<int, LevelAchievement> MappedToLevel = new Dictionary<int, LevelAchievement>();

        public const string ACHIEVEMENTS_KEY = "twr.encodedAchievements";

        public static void LoadAchievements()
        {
            MappedToID.Clear();
            MappedToLevel.Clear();

            foreach (Achievement achievement in All) MappedToID.Add(achievement.id, achievement);
            foreach (LevelAchievement levelAchievement in LevelAchievements) MappedToLevel.Add(levelAchievement.level, levelAchievement);

            string savedString;

            savedString = WRAchievementManager.PersistentData.LoadString(ACHIEVEMENTS_KEY, "");

            if (savedString != "")
            {
                Completed.Clear();

                string[] stringIds = savedString.Split(' ');
                int[] ids = new int[stringIds.Length];

                for (int i = 0; i < stringIds.Length; i++)
                    ids[i] = int.Parse(stringIds[i]);

                foreach (int id in ids)
                    Completed.Add(MappedToID[id]);
            }
        }

        public static void SaveAchievements()
        {
            string saveString = "";

            foreach (Achievement achievement in Completed)
            {
                saveString += achievement.id + " ";
            }

            WRAchievementManager.PersistentData.SaveInt("version", 1);
            WRAchievementManager.PersistentData.SaveString(ACHIEVEMENTS_KEY, saveString.Trim());
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

        public static void ForceCompleteAchievement(int id)
        {
            if (!Completed.Contains(MappedToID[id]))
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

    [Serializable]
    public class LevelAchievement : Achievement
    {
        public int level;
    }
}