using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log(id + ";; " + Achievements.MappedToID.Count + "; " + Achievements.MappedToID);
        achievementToast.GetComponent<AchievementItem>().achievement = Achievements.MappedToID[id];
        achievementToast.GetComponent<Animator>().SetTrigger("ShowToast");
    }
}
