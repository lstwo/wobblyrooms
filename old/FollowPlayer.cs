using ModWobblyLife;
using ModWobblyLife.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldWobblyrooms
{
    public class FollowPlayer : MonoBehaviour
    {
        public static ModPlayerController playerController;

        private void Start()
        {
            ModPlayerController[] controllers = ModInstance.Instance.GetModPlayerControllers();

            foreach (ModPlayerController controller in controllers)
                if (controller.modNetworkObject.IsOwner())
                    playerController = controller;
        }

        void Update()
        {
            ModPlayerController[] controllers = ModInstance.Instance.GetModPlayerControllers();

            foreach (ModPlayerController controller in controllers) if (controller.modNetworkObject.IsOwner()) playerController = controller;

            if (playerController != null)
                transform.position = playerController.GetPlayerTransform().position;
            else Debug.LogError("PlayerController could not be found!");
        }
    }
}