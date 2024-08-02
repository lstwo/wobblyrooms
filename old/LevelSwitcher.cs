using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OldWobblyrooms
{
    public class LevelSwitcher : ModActionInteract
    {
        public int level;
        public NetworkManager networkManager;

        protected override void OnInteract(ModPlayerController playerController)
        {
            NetworkManager.instance.ServerLoadScene(level);
        }
    }
}
