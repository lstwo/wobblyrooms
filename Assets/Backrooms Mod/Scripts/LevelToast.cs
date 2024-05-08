using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelToast : MonoBehaviour
{
    void Awake()
    {
        if(!Settings.toasts) gameObject.SetActive(false);
    }
}
