using ModWobblyLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Camera : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(!FollowPlayer.playerController) yield return null;
        ModCameraFocusSimpleLookAt cameraFocus = FollowPlayer.playerController.GetPlayerCharacter().gameObject.transform.Find("Head").gameObject.AddComponent<ModCameraFocusSimpleLookAt>();
        FollowPlayer.playerController.SetOwnerCameraFocus(cameraFocus);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
