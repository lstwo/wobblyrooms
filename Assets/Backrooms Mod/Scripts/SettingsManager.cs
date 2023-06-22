using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void setHallucinations(bool enabled)
    {
        Settings.hallucinations = enabled;
    }

    public void setAmbience(bool enabled)
    {
        Settings.ambience = enabled;
    }
}

public static class Settings
{
    public static bool hallucinations;
    public static bool ambience;
}
