using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public GameSaveManager saveManager;

    public void SetSave(GameSaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    public void Save()
    {
        saveManager.SaveSettings();
    }
}
