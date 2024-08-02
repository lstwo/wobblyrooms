using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms
{
    public class LevelToast : MonoBehaviour
    {
        void Awake()
        {
            if (!Settings.toasts) gameObject.SetActive(false);
        }
    }
}