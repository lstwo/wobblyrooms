using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wobblyrooms
{
    public class Lights : MonoBehaviour
    {
        void Start()
        {
            if (NetworkManager.temp) gameObject.SetActive(false);
        }
    }
}