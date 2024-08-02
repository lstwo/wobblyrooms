using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OldWobblyrooms.MainMenu
{
    public class AchievementItem : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI _name;
        public TextMeshProUGUI description;

        public Achievement achievement;

        public void Load()
        {
            image.sprite = achievement.image;
            _name.text = achievement.name;
            description.text = achievement.description;
        }
    }
}