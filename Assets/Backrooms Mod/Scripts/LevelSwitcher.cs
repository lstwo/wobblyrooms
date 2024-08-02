using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wobblyrooms
{
    public class LevelSwitcher : ModActionInteract
    {
        public int level;
        public NetworkingManager networkManager;

        protected override void OnInteract(ModPlayerController playerController)
        {
            GameMode.Instance.LoadLevel(level);
        }
    }
}
