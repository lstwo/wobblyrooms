using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OldWobblyrooms.Level0
{
    public class AutoDisable : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(false);
        }
    }
}