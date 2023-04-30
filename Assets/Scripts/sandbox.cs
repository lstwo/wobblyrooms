using ModWobblyLife;

public class sandbox : ModFreemodeGamemode
{
    protected override void OnSpawnedPlayerController(ModPlayerController playerController)
    {
        base.OnSpawnedPlayerController(playerController);

        playerController.ServerSetSandboxUIEnabled(true);
    }
}
