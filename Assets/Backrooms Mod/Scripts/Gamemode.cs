using UnityEngine;
using ModWobblyLife;
using UnityEngine.SceneManagement;
using System;
using IngameDebugConsole;
using UnityEngine.Events;

public class Gamemode : ModFreemodeGamemode
{
    public static Gamemode instance = new Gamemode();
    public Transform playerTransform;

    private void Start()
    {
        DebugLogConsole.AddCommand("backrooms.changeLevel", "Changes the Level you are currently in.", (Action<int>)ChangeLevel, "LevelNumber");
        DebugLogConsole.AddCommand("backrooms.newSeed", "Generates a new Seed for the random generation.", ResetSeed);
        DebugLogConsole.AddCommand("backrooms.jumpscares", "Set whether or not to use jumpscares.", (Action<bool>)Settings.SetJumpscares, "EnableJumpscares");
        DebugLogConsole.AddCommand("backrooms.jumpscares", "Set whether or not to use jumpscares.", GetJumpscares);

        SceneManager.activeSceneChanged += ResetConsoleCommands;
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

    static void GetJumpscares()
    {
        Debug.Log(Settings.jumpscares);
    }

    static void ResetConsoleCommands(Scene a, Scene b)
    {
        DebugLogConsole.RemoveCommand("backrooms.changeLevel");
        DebugLogConsole.RemoveCommand("backrooms.newSeed");
        DebugLogConsole.RemoveCommand("backrooms.jumpscares");
    }

    protected override void OnSpawnedPlayerController(ModPlayerController playerController)
    {
        base.OnSpawnedPlayerController(playerController);

        playerController.ServerSetAllowedCustomClothingAbilities(false);
        instance.playerTransform = playerController.GetPlayerTransform();
        playerController.onPlayerCharacterSpawned += ResetPlayerTransform;
    }

    void ResetPlayerTransform(ModPlayerController controller, ModPlayerCharacter playerCharacter)
    {
        instance.playerTransform = playerCharacter.transform;
    }

    protected override void OnSpawnedPlayerCharacter(ModPlayerController playerController, ModPlayerCharacter playerCharacter)
    {
        base.OnSpawnedPlayerCharacter(playerController, playerCharacter);
        instance.playerTransform = playerCharacter.transform;

        /*if(playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>())
         {
           GameObject go = playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>().gameObject;
           Destroy(playerCharacter.GetComponentInChildren<CameraFocusPlayerCharacter>());
           go.AddComponent<CustomCamFocus>();
           playerController.SetOwnerCameraFocus(go.GetComponent<CustomCamFocus>());
        }*/
    }
    protected override void ModAwake()
    {
        base.ModAwake();

        Debug.Log("\n   _____   _____    ______   _____    _____   _______    _____     \r\n  / ____| |  __ \\  |  ____| |  __ \\  |_   _| |__   __|  / ____|  _ \r\n | |      | |__) | | |__    | |  | |   | |      | |    | (___   (_)\r\n | |      |  _  /  |  __|   | |  | |   | |      | |     \\___ \\     \r\n | |____  | | \\ \\  | |____  | |__| |  _| |_     | |     ____) |  _ \r\n  \\_____| |_|  \\_\\ |______| |_____/  |_____|    |_|    |_____/  (_)\r\n                                                                   \r\n                                                                   \n" +
            "\nMain: LESHADDOW2\nAdditional Help: Shadow404");
    }
}