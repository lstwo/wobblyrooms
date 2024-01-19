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
    public bool jumpscares = true;

    private void Start()
    {
        instance.jumpscares = PlayerPrefs.GetInt("jumpscares") == 1;
        DebugLogConsole.AddCommand("backrooms.changeLevel", "Changes the Level you are currently in.", (Action<int>)ChangeLevel, "LevelNumber");
        DebugLogConsole.AddCommand("backrooms.newSeed", "Generates a new Seed for the random generation.", ResetSeed);
        DebugLogConsole.AddCommand("backrooms.jumpscares", "Set whether or not to use jumpscares.", (Action<bool>)SetJumpscares, "EnableJumpscares");
        DebugLogConsole.AddCommand("backrooms.jumpscares", "Set whether or not to use jumpscares.", GetJumpscares);

        SceneManager.activeSceneChanged += ResetConsoleCommands;
    }

    static void ChangeLevel(int level)
    {
        NetworkManager.instance.ServerLoadScene(level);
    }

    static void ResetSeed()
    {
        int seed = UnityEngine.Random.Range(0, int.MaxValue);
        GameSaves.save1.seed = seed;
        NetworkManager.instance.ServerGenSeed();
    }

    static void SetJumpscares(bool b)
    {
        instance.jumpscares = b;
        if(b) PlayerPrefs.SetInt("jumpscares", 1);
        else PlayerPrefs.SetInt("jumpscares", 0);
    }

    static void GetJumpscares()
    {
        Debug.Log(instance.jumpscares);
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
        playerController.SetAllowedToRespawn(false);
    }

    void ResetPlayerTransform(ModPlayerController controller, ModPlayerCharacter playerCharacter)
    {
        instance.playerTransform = playerCharacter.transform;
    }

    protected override void OnSpawnedPlayerCharacter(ModPlayerController playerController, ModPlayerCharacter playerCharacter)
    {
        base.OnSpawnedPlayerCharacter(playerController, playerCharacter);
        instance.playerTransform = playerCharacter.transform;
    }

}