
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSaveManager : MonoBehaviour
{
    public Image gameImage;
    public TextMeshProUGUI gameName;
    public int saveNumber;
    public AssignLists imageAssigns;

    public TextMeshProUGUI levelText;
    public TMP_InputField nameInput;
    public TMP_InputField seedInput;
    public TMP_InputField levelInput;
    public SaveButton saveButton;
    public Toggle hardcoreToggle;

    public Toggle levelTriggerOptionFreeplay;
    public TMP_InputField levelInputFreeplay;

    GameSave save = new GameSave("unknown_save");

    private void Awake()
    {
        GameSaves.UpdateSaveGet(GameSaves.GetSave(saveNumber));
        save = GameSaves.GetSave(saveNumber);
        if(gameImage != null && imageAssigns.spriteList.ContainsKey(save.level))
            gameImage.sprite = imageAssigns.spriteList[save.level];
        if(gameName != null)
            gameName.text = save.name;
    }

    private void OnEnable()
    {
        GameSaves.UpdateSaveGet(GameSaves.GetSave(saveNumber));
        save = GameSaves.GetSave(saveNumber);
        if (gameImage != null && imageAssigns.spriteList.ContainsKey(save.level))
            gameImage.sprite = imageAssigns.spriteList[save.level];
        if (gameName != null)
            gameName.text = save.name;
    }

    public void LoadSettings()
    {
        levelText.text = "Level: " + save.level;
        nameInput.text = save.name;
        seedInput.text = "" + save.seed;
        levelInput.text = "" + save.level;
        saveButton.saveManager = this;
        hardcoreToggle.isOn = save.hardcore;
    }

    public void LoadSave()
    {
        Settings.enableExits = true;
        GameSaves.currentSave = saveNumber;
        GameSaves.LoadSave(save);
    }

    public void LoadFreeplay()
    {
        GameSaves.currentSave = saveNumber;
        Settings.enableExits = levelTriggerOptionFreeplay.isOn;
        NetworkManager.instance.ServerGenSeed(true);
        NetworkManager.instance.ServerLoadScene(int.Parse(levelInputFreeplay.text));
    }

    public void SaveSettings()
    {
        save.seed = int.Parse(seedInput.text);
        save.name = nameInput.text;
        if(imageAssigns.keys.Contains(int.Parse(levelInput.text)))
            save.level = int.Parse(levelInput.text);
        save.hardcore = hardcoreToggle.isOn;

        gameImage.sprite = imageAssigns.spriteList[save.level];
        gameName.text = save.name;

        GameSaves.SaveGame(GameSaves.GetSave(saveNumber), save);
    }

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

    public static void SaveGame(GameSave save, int level, string name, int seed, bool hardcore)
    {
        Debug.Log("SAVE GAME");
        save.level = level;
        save.name = name;
        save.seed = seed;
        save.hardcore = hardcore;
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
        PlayerPrefs.SetString(save.pprefsKeyPrefix + "_name", save.name);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_seed", save.seed);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
    }

    public static void SaveGame(GameSave save, GameSave other)
    {
        Debug.Log("SAVE GAME");

        save.level = other.level;
        save.name = other.name;
        save.seed = other.seed;
        save.hardcore = other.hardcore;
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
        PlayerPrefs.SetString(save.pprefsKeyPrefix + "_name", save.name);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_seed", save.seed);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
    }

    public static void LoadSave(GameSave save)
    {
        Debug.Log("LOAD SAVE");
        save.level = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_level");
        save.name = PlayerPrefs.GetString(save.pprefsKeyPrefix + "_name");
        save.hardcore = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_hardcore") == 1;
        save.seed = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_seed");
        NetworkManager.instance.ServerGenSeed(false);
        NetworkManager.instance.ServerLoadScene(save.level);
    }

    public static void UpdateSaveGet(GameSave save)
    {
        Debug.Log("UPDATE SAVE GET");
        save.level = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_level");
        save.name = PlayerPrefs.GetString(save.pprefsKeyPrefix + "_name");
        save.seed = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_seed");
        save.hardcore = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_hardcore") == 1;
    }

    public static void UpdateSaveSet(GameSave save)
    {
        Debug.Log("UPDATE SAVE SET");
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
        PlayerPrefs.SetString(save.pprefsKeyPrefix + "_name", save.name);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_seed", save.seed);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_hardcore", save.hardcore ? 1 : 0);
    }

    public static GameSave GetSave(int saveNumber)
    {
        if (saveNumber == 1) return save1; else if(saveNumber == 2) return save2; else if(saveNumber == 3) return save3; else if(saveNumber == 4) return save4;
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
