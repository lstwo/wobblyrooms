using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public ModUIPlayerBasedCanvas ui;
    private void Awake()
    {
        ui.Show();
        ui.SetCursorVisible(true);
    }

}
