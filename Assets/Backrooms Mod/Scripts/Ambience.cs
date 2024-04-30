using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    void Awake()
    {
        if(!Settings.ambience) gameObject.SetActive(false);
    }
}
