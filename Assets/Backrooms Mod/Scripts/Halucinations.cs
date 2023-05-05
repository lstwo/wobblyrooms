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
        if (!Instance.instance.IsValid())
        {
            if (!string.IsNullOrEmpty(sound))
            {
                Instance.instance = ModRuntimeManager.CreateInstance(sound);
                ModRuntimeManager.AttachInstanceToGameObject(Instance.instance, transform, (Rigidbody)null);
                Instance.instance.Start();
            }
        }
    }

    void OnDisable()
    {
        if (Instance.instance.IsValid())
        {
            Instance.instance.Stop(MOD_STOP_MODE.ALLOWFADEOUT);
            Instance.instance.Release();
            Instance.instance.ClearHandle();
        }
    }

    void FixedUpdate()
    {
        Instance.counter += Time.deltaTime;
        Debug.Log(Instance.counter);

        if (!Instance.isHalucinating && Instance.counter >= Random.Range(60, 960))
        {
            Instance.counter = -13;
            Instance.isHalucinating = true;
            Debug.Log("halucination start");
        } 

        if(Instance.isHalucinating)
        {
            Instance.instance.SetParameterByName("volume", Instance.counter);
            Debug.Log("halucination tick");
        }

        if(Instance.isHalucinating && Instance.counter >= 10)
        {
            Instance.instance.SetParameterByName("volume", -13);
            Instance.counter = 0;
            Instance.isHalucinating = false;
            Debug.Log("halucination end");
        }
    }
}

