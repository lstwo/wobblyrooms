using ModWobblyLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Wobblyrooms
{
    public class GameMode : ModFreemodeGamemode
    {
        [Header("Assigns")]

        [Tooltip("The Main Generation Manager for this Level")]
        public BaseGenerationManager generationManager;

        [Tooltip("The Networking Manager in for this Scene")]
        public NetworkingManager networkingManager;


        [HideInInspector]
        public List<ModPlayerController> playerControllers = new List<ModPlayerController>();

        [HideInInspector]
        public List<Transform> playerTransforms = new List<Transform>();

        private static GameMode _instance;

        [HideInInspector]
        public static GameMode Instance { get { return _instance; } }


        protected override void ModAwake()
        {
            Debug.Log(generationManager + "asfkjhksdhdgm");
            Debug.Log(networkingManager + "asfkjhksdhdgm");

            base.ModAwake();

            _instance = this;

            if(generationManager != null)
                StartCoroutine(generationManager.AwakeGen());
        }

        protected override void ModStart()
        {
            Debug.Log(generationManager + "asfkjhksdhdgm");
            Debug.Log(networkingManager + "asfkjhksdhdgm");
            base.ModStart();

            if(generationManager != null)
                StartCoroutine(generationManager.StartGen());
        }

        protected override void Update()
        {
            base.Update();

            if(generationManager != null)
                StartCoroutine(generationManager.UpdateGen(playerControllers));
        }

        protected override void OnSpawnedPlayerController(ModPlayerController playerController)
        {
            base.OnSpawnedPlayerController(playerController);

            playerControllers.Add(playerController);
        }

        protected override void OnSpawnedPlayerCharacter(ModPlayerController playerController, ModPlayerCharacter playerCharacter)
        {
            base.OnSpawnedPlayerCharacter(playerController, playerCharacter);

            playerTransforms.Add(playerCharacter.GetPlayerTransform());
        }

        /// <summary>
        /// Loads a level named "Level <paramref name="level"/>"
        /// </summary>
        /// <param name="level"></param>
        public void LoadLevel(int level)
        {
            networkingManager.ServerLoadScene("Level " + level);
        }
    }
}
