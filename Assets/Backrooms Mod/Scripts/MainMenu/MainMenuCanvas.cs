using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.UI;
using UnityEngine.InputSystem;
using System;

public class MainMenuCanvas : MonoBehaviour
{
    public AchievementManager AchievementManager;
    public GameObject temp;
    public int tempNum = 3;

    private void Start()
    {
        AchievementManager.OnEnable();
        if(temp != null && (NetworkManager.num / 4 == tempNum || NetworkManager.num / 4 == tempNum - 1 || NetworkManager.num / 4 == tempNum + 1))
        {
            NetworkManager.temp = true;
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
