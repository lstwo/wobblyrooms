using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
{
    public class LevelToast : MonoBehaviour
    {
        void Awake()
        {
            if (!Settings.toasts) gameObject.SetActive(false);
        }
    }
}