using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UMod;

namespace Wobblyrooms.MainMenu
{
    public class GameSaveManager : ModScriptBehaviour
    {
        public static IModPersistentData PersistentData { get; private set; }

        [Header("Game Save Info Card")]
        public Image gameImage;
        public TextMeshProUGUI gameName;
        public int saveNumber;
        public AssignLists imageAssigns;

        [Header("Game Save Info Card")]
        public TextMeshProUGUI levelText;
        public TMP_InputField nameInput;
        public TMP_InputField seedInput;
        public TMP_InputField levelInput;
        public SaveButton saveButton;
        public Toggle hardcoreToggle;

        GameSave save = new GameSave("unknown_save");

        public override void OnModLoaded()
        {
            base.OnModLoaded();

            PersistentData = ModPersistentData;
        }

        private void Awake()
        {
            if (imageAssigns != null) imageAssigns.Awake();

            GameSaves.UpdateSaveGet(GameSaves.GetSave(saveNumber));
            save = GameSaves.GetSave(saveNumber);

            if (gameImage != null && imageAssigns.spriteList.ContainsKey(save.level))
                gameImage.sprite = imageAssigns.spriteList[save.level];

            if (gameName != null)
                gameName.text = save.name;

            NetworkingManager.SetNum(DateTime.Now.Hour * 4);
        }

        private void OnEnable()
        {
            if (imageAssigns != null) imageAssigns.Awake();

            GameSaves.UpdateSaveGet(GameSaves.GetSave(saveNumber));
            save = GameSaves.GetSave(saveNumber);

            if (gameImage != null && imageAssigns.spriteList.ContainsKey(save.level))
                gameImage.sprite = imageAssigns.spriteList[save.level];

            if (gameName != null)
                gameName.text = save.name;
        }

        /// <summary>
        /// Sets the values on the settings ui elements
        /// </summary>
        public void LoadSettings()
        {
            levelText.text = "Level: " + save.level;
            nameInput.text = save.name;
            seedInput.text = "" + save.seed;
            levelInput.text = "" + save.level;
            saveButton.saveManager = this;
            hardcoreToggle.isOn = save.hardcore;
        }

        /// <summary>
        /// Loads the save of the manager
        /// </summary>
        public void LoadSave()
        {
            Settings.enableExits = true;
            GameSaves.currentSave = saveNumber;
            GameSaves.LoadSave(save);
        }

        /// <summary>
        /// Saves the settings to the Game save
        /// </summary>
        public void SaveSettings()
        {
            save.seed = int.Parse(seedInput.text);
            save.name = nameInput.text;
            if (imageAssigns.keys.Contains(int.Parse(levelInput.text)))
                save.level = int.Parse(levelInput.text);
            save.hardcore = hardcoreToggle.isOn;

            gameImage.sprite = imageAssigns.spriteList[save.level];
            gameName.text = save.name;

            GameSaves.SaveGame(GameSaves.GetSave(saveNumber), save);
        }

        /// <summary>
        /// Resets the save to default
        /// </summary>
        public void ResetSave()
        {
            save.seed = 0;
            save.name = "Unnamed Save";
            save.level = 0;

            gameImage.sprite = imageAssigns.spriteList[save.level];
            gameName.text = save.name;

            GameSaves.SaveGame(GameSaves.GetSave(saveNumber), save);
        }
    }

    public static class GameSaves
    {
        public static GameSave save1 = new GameSave("save_one");
        public static GameSave save2 = new GameSave("save_two");
        public static GameSave save3 = new GameSave("save_three");
        public static GameSave save4 = new GameSave("save_four");

        public static int currentSave = 0;

        /// <summary>
        /// Sets the values on <paramref name="save"/>
        /// </summary>
        /// <param name="save">save to write to</param>
        /// <param name="level">new level number to save</param>
        /// <param name="name">name of the save</param>
        /// <param name="seed">seed of the save</param>
        /// <param name="hardcore">whether the save has hardcore enabled</param>
        public static void SaveGame(GameSave save, int level, string name, int seed, bool hardcore)
        {
            save.level = level;
            save.name = name;
            save.seed = seed;
            save.hardcore = hardcore;

            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_level", save.level);
            GameSaveManager.PersistentData.SaveString(save.pprefsKeyPrefix + "_name", save.name);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_seed", save.seed);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
        }

        /// <summary>
        /// Sets the values on <paramref name="save"/> to the values of <paramref name="other"/>
        /// </summary>
        /// <param name="save"></param>
        /// <param name="other"></param>
        public static void SaveGame(GameSave save, GameSave other)
        {
            save.level = other.level;
            save.name = other.name;
            save.seed = other.seed;
            save.hardcore = other.hardcore;

            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_level", save.level);
            GameSaveManager.PersistentData.SaveString(save.pprefsKeyPrefix + "_name", save.name);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_seed", save.seed);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
        }

        /// <summary>
        /// Loads <paramref name="save"/> and loads the level
        /// </summary>
        /// <param name="save"></param>
        public static void LoadSave(GameSave save)
        {
            save.level = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_level");
            save.name = PlayerPrefs.GetString(save.pprefsKeyPrefix + "_name");
            save.hardcore = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_hardcore") == 1;
            save.seed = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_seed");
            GameMode.Instance.networkingManager.ServerGenSeed(false);
            GameMode.Instance.LoadLevel(save.level);
        }

        /// <summary>
        /// Updates the current local save by getting the player prefs save.
        /// </summary>
        /// <param name="save"></param>
        public static void UpdateSaveGet(GameSave save)
        {
            save.level = GameSaveManager.PersistentData.LoadInt(save.pprefsKeyPrefix + "_level");
            save.name = GameSaveManager.PersistentData.LoadString(save.pprefsKeyPrefix + "_name", "Unnamed Save");
            save.seed = GameSaveManager.PersistentData.LoadInt(save.pprefsKeyPrefix + "_seed");
            save.hardcore = GameSaveManager.PersistentData.LoadInt(save.pprefsKeyPrefix + "_hardcore") == 1;
        }

        /// <summary>
        /// Updates the player prefs save with the values from <paramref name="save"/>
        /// </summary>
        /// <param name="save"></param>
        public static void UpdateSaveSet(GameSave save)
        {
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_level", save.level);
            GameSaveManager.PersistentData.SaveString(save.pprefsKeyPrefix + "_name", save.name);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_seed", save.seed);
            GameSaveManager.PersistentData.SaveInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
        }

        /// <summary>
        /// Gets the save from <paramref name="save"/>
        /// </summary>
        /// <param name="saveNumber"></param>
        /// <returns></returns>
        public static GameSave GetSave(int saveNumber)
        {
            if (saveNumber == 1) return save1;
            else if (saveNumber == 2) return save2;
            else if (saveNumber == 3) return save3;
            else if (saveNumber == 4) return save4;
            else return null;
        }
    }

    public class GameSave
    {
        public string pprefsKeyPrefix = "default_save_name";
        public string name = "Unnamed Save";

        public int level = 0;
        public int seed = 0;

        public bool hardcore = false;

        public GameSave(string pprefsKeyPrefix)
        {
            this.pprefsKeyPrefix = pprefsKeyPrefix;
        }
    }
}