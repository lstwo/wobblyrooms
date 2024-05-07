using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSwitcher : ModActionInteract
{
    public int level;
    public NetworkManager networkManager;

    protected override void OnInteract(ModPlayerController playerController)
    {
        Achievements.CompleteAchievement(Achievements.LevelAchievements[level].id);
        NetworkManager.instance.ServerLoadScene(level);
    }
}
