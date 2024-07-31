using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class Ambience : MonoBehaviour
    {
        void Awake()
        {
            if (!Settings.ambience) gameObject.SetActive(false);
        }
    }
}