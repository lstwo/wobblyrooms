using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Wobblyrooms.MainMenu
{
    public class FreeplayGameSaveManager : GameSaveManager
    {
        public Toggle levelTriggerOptionFreeplay;
        public TMP_InputField levelInputFreeplay;

        public void LoadFreeplay()
        {
            GameSaves.currentSave = saveNumber;
            Settings.enableExits = levelTriggerOptionFreeplay.isOn;
            GameMode.Instance.networkingManager.ServerGenSeed(true);
            GameMode.Instance.networkingManager.ServerLoadScene("Level " + int.Parse(levelInputFreeplay.text));
        }
    }
}
