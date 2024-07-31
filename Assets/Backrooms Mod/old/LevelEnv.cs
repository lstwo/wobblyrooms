using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wobblyrooms
{
    public class LevelEnv : MonoBehaviour
    {
        void Start()
        {
            if (NetworkManager.temp) RenderSettings.ambientLight = Color.black;
        }
    }
}