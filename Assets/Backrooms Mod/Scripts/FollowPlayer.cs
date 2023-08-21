using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    void Update()
    {
        if(Gamemode.instance.playerTransform != null)
            transform.position = Gamemode.instance.playerTransform.position;
    }
}
