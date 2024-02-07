using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : ModCameraFocusSimpleLookAt
{
    private ModPlayerController controller;

    public override void UpdateCamera(ModGameplayCamera camera, Transform cameraTransform)
    {
        if(controller)
            cameraTransform.position = controller.GetPlayerCharacter().gameObject.transform.Find("Head").position;
    }

    protected override void OnFocusCamera(ModPlayerController playerController)
    {
        controller = playerController;
    }

    protected override void OnUnfocusCamera(ModPlayerController playerController)
    {
        controller = null;
    }
}
