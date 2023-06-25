using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public ModUIPlayerBasedCanvas ui;
    public ModUIPlayerBasedElement mainMenu, saveSettings, saveSettingsIndiv, loadSave, settings;
    private void Awake()
    {
        ui.Show();
        ui.SetCursorVisible(true);
        mainMenu.Show();
    }

    public void showSaveSettings() { saveSettings.Show(); loadSave.Hide(); settings.Hide(); }
    public void showSaveSettingsIndiv() { saveSettingsIndiv.Show(); }
    public void showLoadSave() { loadSave.Show(); saveSettings.Hide(); settings.Hide(); }
    public void showSettings() { settings.Show(); saveSettings.Hide(); loadSave.Hide(); }

}
