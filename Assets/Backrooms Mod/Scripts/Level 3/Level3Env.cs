using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Env : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.temp) RenderSettings.ambientLight = Color.black;
    }
}
