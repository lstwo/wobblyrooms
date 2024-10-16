using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms
{
    public class AchievementTrigger : MonoBehaviour
    {
        public int id = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Achievements.CompleteAchievement(id);
            }
        }
    }
}