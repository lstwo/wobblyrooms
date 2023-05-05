using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;

public class NoClothingAbilities : ModFreemodeGamemode
{
    protected override void OnSpawnedPlayerController(ModPlayerController playerController)
    {
        base.OnSpawnedPlayerController(playerController);

        playerController.ServerSetAllowedCustomClothingAbilities(false);
    }
}
