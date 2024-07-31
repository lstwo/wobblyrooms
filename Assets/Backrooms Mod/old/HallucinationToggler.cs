using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms
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