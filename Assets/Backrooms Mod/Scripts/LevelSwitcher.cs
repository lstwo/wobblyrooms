using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSwitcher : MonoBehaviour
{
    public int level;

    private void OnTriggerEnter(Collider other)
    {
        GameSaves.SaveGame(GameSaves.GetSave(GameSaves.currentSave), level, GameSaves.GetSave(GameSaves.currentSave).name, GameSaves.GetSave(GameSaves.currentSave).seed);
        SceneManager.LoadScene("Level " + level);
    }
}
