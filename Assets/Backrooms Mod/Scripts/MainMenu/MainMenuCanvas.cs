using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.UI;
using UnityEngine.InputSystem;
using System;

namespace Wobblyrooms.MainMenu
{
    public class MainMenuCanvas : MonoBehaviour
    {
        public WRAchievementManager AchievementManager;
        public GameObject temp;
        public int tempNum = 3;

        private void Start()
        {
            AchievementManager.OnEnable();
            if (temp != null && (NetworkingManager.num / 4 == tempNum || NetworkingManager.num / 4 == tempNum - 1 || NetworkingManager.num / 4 == tempNum + 1))
            {
                Achievements.CompleteAchievement(14);
                NetworkingManager.temp = true;
                temp.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}