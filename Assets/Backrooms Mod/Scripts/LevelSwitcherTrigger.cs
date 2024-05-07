using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSwitcherTrigger : MonoBehaviour
{
    public int level;
    public NetworkManager networkManager;

    private void OnTriggerEnter(Collider other)
    {
        if(Settings.enableExits)
        {
            Achievements.CompleteAchievement(Achievements.LevelAchievements[level].id);
            NetworkManager.instance.ServerLoadScene(level);
        }
        else
        {
            ModPlayerController[] controllers = ModInstance.Instance.GetModPlayerControllers();

            foreach (ModPlayerController controller in controllers)
                if (controller.modNetworkObject.IsOwner())
                    controller.GetPlayerCharacter().Kill();
        }
    }
}
