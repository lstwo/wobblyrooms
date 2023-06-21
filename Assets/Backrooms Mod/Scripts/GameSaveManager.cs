using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSaveManager : MonoBehaviour
{
    public Image gameImage;
    public Text gameName;
    public int saveNumber;
    public AssignLists imageAssigns;

    GameSave save = new GameSave("unknown_save");

    private void Awake()
    {
        if (saveNumber == 1) save = GameSaver.save1; else if (saveNumber == 2) save = GameSaver.save2; else if (saveNumber == 3) save = GameSaver.save3;
        gameImage.sprite = imageAssigns.spriteList[save.level];
        gameName.text = save.name;
    }

    void LoadSave()
    {
        GameSaver.currentSave = saveNumber;
    }
}

public static class GameSaver
{
    public static GameSave save1 = new GameSave("save_one");
    public static GameSave save2 = new GameSave("save_two");
    public static GameSave save3 = new GameSave("save_three");

    public static int currentSave = 1;

    public static void SaveGame(GameSave save, int level, string name, int seed)
    {
        save.level = level;
        save.name = name;
        save.seed = seed;
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
        PlayerPrefs.SetString(save.pprefsKeyPrefix + "_name", save.name);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_seed", save.seed);

    }

    public static void LoadSave(GameSave save)
    {
        save.level = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_level");
        save.name = PlayerPrefs.GetString(save.pprefsKeyPrefix + "_name");
        SceneManager.LoadScene("Level " + save.level);
    }

    public static void UpdateSaveGet(GameSave save)
    {
        save.level = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_level");
        save.name = PlayerPrefs.GetString(save.pprefsKeyPrefix + "_name");
        save.seed = PlayerPrefs.GetInt(save.pprefsKeyPrefix + "_seed");
    }

    public static void UpdateSaveSet(GameSave save)
    {
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
        PlayerPrefs.SetString(save.pprefsKeyPrefix + "_name", save.name);
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_seed", save.seed);
    }
}

public class GameSave
{
    public string pprefsKeyPrefix = "default_save_name";
    public string name = "Unnamed Save";

    public int level = 0;
    public int seed = 0;

    public GameSave(string pprefsKeyPrefix)
    {
        this.pprefsKeyPrefix = pprefsKeyPrefix;
    }
}
