using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

namespace Wobblyrooms.Level4
{
    public class Level4Generator : HallwayGenManager
    {
        [Header("Interiors")]

        [Tooltip("All interior prefabs. Put duplicates for higher chance")]
        public GameObject[] interiors;

        public override IEnumerator AwakeGen()
        {
            yield return base.AwakeGen();

            startSegmentOffset = -lastSegmentStart.up * (8*1.3f);
            endSegmentOffset = Vector3.zero;
        }

        public override void OnSegmentEndGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {
            base.OnSegmentEndGenerated(behaviour, spawnPosition);

            if (Random.Range(0, 4) < 3)
            {
                int rand = Random.Range(0, interiors.Length);
                ModNetworkManager.Instance.InstantiateNetworkPrefab(interiors[rand], null, spawnPosition, lastSegmentEnd.rotation, null, true);
            }
        }

        public override void OnSegmentStartGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {
            base.OnSegmentStartGenerated(behaviour, spawnPosition);

            if (Random.Range(0, 4) < 3)
            {
                int rand = Random.Range(0, interiors.Length);
                ModNetworkManager.Instance.InstantiateNetworkPrefab(interiors[rand], null, spawnPosition, lastSegmentStart.rotation, null, true);
            }
        }
    }
}