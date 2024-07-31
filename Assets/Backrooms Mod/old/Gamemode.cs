using UnityEngine;
using ModWobblyLife;
using UnityEngine.SceneManagement;
using System;
using IngameDebugConsole;
using UnityEngine.Events;
using Wobblyrooms.MainMenu;
using System.Collections;

namespace Wobblyrooms
{
    public class Gamemode : ModFreemodeGamemode
    {
        public float hungerRate = .1f;
        public float sanityRate = .075f;
        public float healthRegenRate = .125f;
        public float damageRate = .075f;

        public static Transform playerTransform;

        public static int maxHP = 100;
        public static int maxHunger = 100;
        public static int maxSanity = 100;

        public static int hp = maxHP;
        public static int hunger = maxHunger;
        public static int sanity = maxSanity;

        private void Start()
        {
            DebugLogConsole.AddCommand("backrooms.changeLevel", "Changes the Level you are currently in.", (Action<int>)ChangeLevel, "LevelNumber");

            SceneManager.activeSceneChanged += ResetConsoleCommands;

            StartCoroutine(doHungerSim());
            StartCoroutine(doSanitySim());
            StartCoroutine(doRegen());
            StartCoroutine(doDamage());
        }

        IEnumerator doHungerSim()
        {
            yield return new WaitForSeconds(1 / hungerRate);
            hunger--;
        }

        IEnumerator doSanitySim()
        {
            yield return new WaitForSeconds(1 / sanityRate);
            sanity--;
        }

        IEnumerator doRegen()
        {
            yield return new WaitForSeconds(1 / healthRegenRate);
            if (hunger >= 75) hp++;
        }

        IEnumerator doDamage()
        {
            yield return new WaitForSeconds(1 / damageRate);
            if (hunger <= 25) hp--;
        }

        public static void ChangeLevel(int level)
        {
            NetworkManager.instance.ServerLoadScene(level);
        }

        public static void ResetSeed()
        {
            int seed = UnityEngine.Random.Range(0, int.MaxValue);
            GameSaves.save1.seed = seed;
            NetworkManager.instance.ServerGenSeed(true);
        }

        static void ResetConsoleCommands(Scene a, Scene b)
        {
            DebugLogConsole.RemoveCommand("backrooms.changeLevel");
        }

        protected override void OnSpawnedPlayerController(ModPlayerController playerController)
        {
            base.OnSpawnedPlayerController(playerController);

            playerController.ServerSetAllowedCustomClothingAbilities(false);
            playerTransform = playerController.GetPlayerTransform();
            playerController.onPlayerCharacterSpawned += ResetPlayerTransform;
        }

        void ResetPlayerTransform(ModPlayerController controller, ModPlayerCharacter playerCharacter)
        {
            playerTransform = playerCharacter.transform;
        }

        protected override void OnSpawnedPlayerCharacter(ModPlayerController playerController, ModPlayerCharacter playerCharacter)
        {
            base.OnSpawnedPlayerCharacter(playerController, playerCharacter);
            playerTransform = playerCharacter.transform;

            /*if(playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>())
             {
               GameObject go = playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>().gameObject;
               Destroy(playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>());
               go.AddComponent<CustomCamFocus>();
               playerController.SetOwnerCameraFocus(go.GetComponent<CustomCamFocus>());
            }*/
        }
    }
}