using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Audio;


public class Halucinations : MonoBehaviour
{
  
    [SerializeField] private string sound = "event:/level_0_ambience";

    private ModEventInstance instance;

    private Vector3 previousPosition;

    private float counter = 0;

    void OnEnable()
    {
        previousPosition = transform.position;

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
        counter += Time.deltaTime;
        if (instance.IsValid())
        {
            // Calculates the velocity instead of using Rigidbody.velocity because the client doesn't Simulate physics.
            Vector3 velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;

            instance.SetParameterByName("Speed", velocity.magnitude);
        }
        if (counter >= Random.Range(10, 120))
        {
            counter = -13;
            instance.SetParameterByName("volume", counter);
        } 
            
        previousPosition = transform.position;
    }
}

