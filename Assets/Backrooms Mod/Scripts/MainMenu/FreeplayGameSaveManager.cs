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
            print("dhjfjds");
            print("1;" + GameSaves.currentSave + ";" + saveNumber);
            GameSaves.currentSave = saveNumber;
            print("2;" + Settings.enableExits + ";" + levelTriggerOptionFreeplay);
            Settings.enableExits = levelTriggerOptionFreeplay.isOn;
            print("3;" + GameMode.Instance);
            print("3;" + GameMode.Instance.networkingManager);
            GameMode.Instance.networkingManager.ServerGenSeed(true);
            print("4");
            GameMode.Instance.networkingManager.ServerLoadScene("Level " + int.Parse(levelInputFreeplay.text));
            print("5");
        }
    }
}
