using ModWobblyLife;
using ModWobblyLife.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    void Update()
    {
        ModPlayerController[] controllers = ModInstance.Instance.GetModPlayerControllers();
        ModPlayerController pc = null;

        foreach (ModPlayerController controller in controllers) if(controller.modNetworkObject.IsOwner()) pc = controller;

        if (pc != null) transform.position = pc.GetPlayerTransform().position;
        else Debug.LogError("PlayerController could not be found!");
    }
}
