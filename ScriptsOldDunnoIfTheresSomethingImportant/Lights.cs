using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    void Start()
    {
        if(NetworkManager.temp) gameObject.SetActive(false);
    }
}
