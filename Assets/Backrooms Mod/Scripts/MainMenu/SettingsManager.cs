using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle jumpscareToggle, ambienceToggle, hallucinationToggle;

    public void Start()
    {
        Settings.LoadSettings();
        jumpscareToggle.isOn = Settings.jumpscares;
        ambienceToggle.isOn = Settings.ambience;
        hallucinationToggle.isOn = Settings.hallucinations;
    }

    public void setJumpscares(bool enabled)
    {
        Settings.SetJumpscares(enabled);
    }

    public void setAmbience(bool enabled)
    {
        Settings.SetAmbience(enabled);
    }

    public void setHallucinations(bool enabled)
    {
        Settings.SetHallucinations(enabled);
    }
}

public static class Settings
{
    public static bool jumpscares = true;
    public static bool ambience = true;
    public static bool hallucinations = true;

    public static void LoadSettings()
    {
        jumpscares = PlayerPrefs.GetInt("jumpscares") == 0;
        ambience = PlayerPrefs.GetInt("ambience") == 0;
        hallucinations = PlayerPrefs.GetInt("hallucinations") == 0;
    }

    public static void SetJumpscares(bool b)
    {
        jumpscares = b;
        if (b) PlayerPrefs.SetInt("jumpscares", 0);
        else PlayerPrefs.SetInt("jumpscares", 1);
    }

    public static void SetAmbience(bool b)
    {
        ambience = b;
        if (b) PlayerPrefs.SetInt("ambience", 0);
        else PlayerPrefs.SetInt("ambience", 1);
    }

    public static void SetHallucinations(bool b)
    {
        hallucinations = b;
        if (b) PlayerPrefs.SetInt("hallucinations", 0);
        else PlayerPrefs.SetInt("hallucinations", 1);
    }
}
