using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Audio;

public class Halucinations : MonoBehaviour
{
    [SerializeField] private string sound = "event:/level_0_ambience";
    public static Halucinations Instance = new Halucinations();
    public List<GameObject> doors = new List<GameObject>();

    private ModEventInstance instance;

    private Vector3 previousPosition;

    private float counter = 0;
    private bool isHalucinating = false;

    void OnEnable()
    {
        if (!instance.IsValid())
        {
            if (!string.IsNullOrEmpty(sound))
            {
                instance = ModRuntimeManager.CreateInstance(sound);
                ModRuntimeManager.AttachInstanceToGameObject(instance, transform, (Rigidbody)null);
                instance.Start();
            }
        }
    }

    void OnDisable()
    {
        if (instance.IsValid())
        {
            instance.Stop(MOD_STOP_MODE.ALLOWFADEOUT);
            instance.Release();
            instance.ClearHandle();
        }
    }

    void FixedUpdate()
    {
        Instance.counter += Time.deltaTime;
        Debug.Log(counter);

        if (!Instance.isHalucinating && Instance.counter >= Random.Range(10, 120))
        {
            Instance.counter = -13;
            Instance.isHalucinating = true;
            Debug.Log("halucination start");
        } 

        if(isHalucinating)
        {
            instance.SetParameterByName("volume", counter);
            Debug.Log("halucination tick");
        }

        if(isHalucinating && counter >= 10)
        {
            instance.SetParameterByName("volume", -13);
            Instance.counter = 0;
            Instance.isHalucinating = false;
            Debug.Log("halucination end");
        }
    }
}

