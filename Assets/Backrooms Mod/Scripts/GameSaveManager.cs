using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSaveManager : MonoBehaviour
{
    public Image gameImmage;
    public TextMeshProUGUI gameName;
    public int saveNumber;

    private void Awake()
    {

    }
}

public static class GameSaver
{
    public static GameSave save1 = new GameSave("save_one");
    public static GameSave save2 = new GameSave("save_two");
    public static GameSave save3 = new GameSave("save_three");

    public static void SaveGame(GameSave save, int level)
    {
        save.level = level;
        PlayerPrefs.SetInt(save.pprefsKeyPrefix + "_level", save.level);
    }

    public static void LoadGame(GameSave save)
    {
        SceneManager.LoadScene("Level " + save.level);
    }
}

public class GameSave
{
    public string pprefsKeyPrefix;

    public int level;

    public GameSave(string pprefsKeyPrefix)
    {
        this.pprefsKeyPrefix = pprefsKeyPrefix;
    }
}
