using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms
{
    public class Ambience : MonoBehaviour
    {
        void Awake()
        {
            if (!Settings.ambience) gameObject.SetActive(false);
        }
    }
}