using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Audio;

public class Hallucinations : MonoBehaviour
{
    [SerializeField] private string sound = "event:/level_0_ambience";
    public static Hallucinations Instance = new Hallucinations();
    public List<GameObject> doors = new List<GameObject>();

    private ModEventInstance instance;

    private Vector3 previousPosition;

    private float counter = 0;
    private bool isHallucinating = false;

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

        if (!Instance.isHallucinating && Instance.counter >= Random.Range(60, 960))
        {
            Instance.counter = -13;
            Instance.isHallucinating = true;
        } 

        if(Instance.isHallucinating)
        {
            Instance.instance.SetParameterByName("volume", Instance.counter);
        }

        if(Instance.isHallucinating && Instance.counter >= 10)
        {
            Instance.instance.SetParameterByName("volume", -13);
            Instance.counter = 0;
            Instance.isHallucinating = false;
        }
    }
}

