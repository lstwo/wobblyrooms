using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Achievement achievement;

    public void Load()
    {
        image.sprite = achievement.image;
        name.text = achievement.name;
        description.text = achievement.description;
    }
}
