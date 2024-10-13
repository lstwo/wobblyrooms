using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wobblyrooms.MainMenu
{
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

        public void setEntitiesAllowed(bool enabled)
        {
            Settings.SetEntitiesAllowed(enabled);
        }

        public void setToastsAllowed(bool enabled)
        {
            Settings.SetToastsAllowed(enabled);
        }
    }

    public static class Settings
    {
        public static bool jumpscares = true;
        public static bool ambience = true;
        public static bool hallucinations = true;
        public static bool entities = true;
        public static bool toasts = true;

        public static bool enableExits = true;

        public static void LoadSettings()
        {
            jumpscares = GameSaveManager.PersistentData.LoadInt("jumpscares") == 0;
            ambience = GameSaveManager.PersistentData.LoadInt("ambience") == 0;
            hallucinations = GameSaveManager.PersistentData.LoadInt("hallucinations") == 0;
        }

        public static void SetJumpscares(bool b)
        {
            jumpscares = b;
            if (b) GameSaveManager.PersistentData.SaveInt("jumpscares", 0);
            else GameSaveManager.PersistentData.SaveInt("jumpscares", 1);
        }

        public static void SetAmbience(bool b)
        {
            ambience = b;
            if (b) GameSaveManager.PersistentData.SaveInt("ambience", 0);
            else GameSaveManager.PersistentData.SaveInt("ambience", 1);
        }

        public static void SetHallucinations(bool b)
        {
            hallucinations = b;
            if (b) GameSaveManager.PersistentData.SaveInt("hallucinations", 0);
            else GameSaveManager.PersistentData.SaveInt("hallucinations", 1);
        }

        public static void SetEntitiesAllowed(bool b)
        {
            entities = b;
        }

        public static void SetToastsAllowed(bool b)
        {
            toasts = b;
        }
    }
}