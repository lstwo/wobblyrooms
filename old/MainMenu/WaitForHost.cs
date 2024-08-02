using ModWobblyLife.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldWobblyrooms.MainMenu
{
    public class WaitForHost : MonoBehaviour
    {
        void Start()
        {
            if (ModNetworkManager.Instance.IsServer()) gameObject.SetActive(false);
        }
    }
}