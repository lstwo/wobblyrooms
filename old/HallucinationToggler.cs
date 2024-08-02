using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms
{
    public class HallucinationToggler : MonoBehaviour
    {
        void Start()
        {
            if (!Settings.hallucinations) gameObject.SetActive(false);
        }

        void Update()
        {

        }
    }
}