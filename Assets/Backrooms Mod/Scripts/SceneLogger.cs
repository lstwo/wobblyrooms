using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLogger : MonoBehaviour
{
    float counter = 0;

    void Update()
    {
        counter += Time.deltaTime;

        if(counter > 5)
        {
            counter = 0;
            Debug.Log(SceneManager.GetActiveScene().name);
        }
    }
}
