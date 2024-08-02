using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife.Audio;
using OldWobblyrooms.MainMenu;

namespace OldWobblyrooms.Level0
{
    public class Hallucinations : MonoBehaviour
    {
        [SerializeField] private string sound = "event:/level_0_ambience";

        private ModEventInstance instance;

        private Vector3 previousPosition;

        private float counter = 0;
        private bool isHallucinating = false;

        void OnEnable()
        {
            if (!instance.IsValid() && Settings.ambience)
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
            if (Settings.hallucinations)
            {
                counter += Time.deltaTime;

                if (!isHallucinating && counter >= Random.Range(60, 960))
                {
                    counter = -13;
                    isHallucinating = true;
                }

                if (isHallucinating)
                {
                    instance.SetParameterByName("volume", counter);
                }

                if (isHallucinating && counter >= 10)
                {
                    instance.SetParameterByName("volume", -13);
                    counter = 0;
                    isHallucinating = false;
                    Achievements.CompleteAchievement(10);
                }
            }
        }
    }
}