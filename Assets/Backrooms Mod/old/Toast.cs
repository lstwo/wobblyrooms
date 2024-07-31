using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class Toast : MonoBehaviour
    {
        public GameObject achievementToast;

        void Awake()
        {
            DontDestroyOnLoad(this);
            Achievements.onAchievementUnlocked += ShowToast;
        }

        void ShowToast(int id)
        {
            achievementToast.GetComponent<AchievementItem>().achievement = Achievements.MappedToID[id];
            achievementToast.GetComponent<AchievementItem>().Load();
            achievementToast.GetComponent<Animation>().Play();
        }
    }
}
