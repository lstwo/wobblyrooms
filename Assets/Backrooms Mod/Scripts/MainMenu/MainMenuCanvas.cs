using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.UI;
using UnityEngine.InputSystem;

public class MainMenuCanvas : MonoBehaviour
{
    public AchievementManager AchievementManager;

    private void Awake()
    {
        AchievementManager.OnEnable();
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
