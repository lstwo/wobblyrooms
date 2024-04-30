using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinationToggler : MonoBehaviour
{
    void Start()
    {
        if(!Settings.hallucinations) gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
