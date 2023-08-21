using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;

public class Gamemode : ModFreemodeGamemode
{
    public static Gamemode instance = new Gamemode();
    public Transform playerTransform;

    protected override void OnSpawnedPlayerController(ModPlayerController playerController)
    {
        base.OnSpawnedPlayerController(playerController);

        playerController.ServerSetAllowedCustomClothingAbilities(false);
        instance.playerTransform = playerController.GetPlayerTransform();
    }

    protected override void OnServerPlayerDied(ModPlayerController playerController, ModPlayerCharacter playerCharacter)
    {
        base.OnServerPlayerDied(playerController, playerCharacter);
        instance.playerTransform = playerController.GetPlayerTransform();
    }
}
