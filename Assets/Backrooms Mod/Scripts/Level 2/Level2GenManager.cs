using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;
using Wobblyrooms.MainMenu;

namespace Wobblyrooms.Level2
{
    public class Level2GenManager : HallwayGenManager
    {
        [Header("Monster Options")]

        [Tooltip("Monster Prefab for end segments")]
        public GameObject moncherPrefabEnd;

        [Tooltip("Monster Prefab for start segments")]
        public GameObject moncherPrefabStart;

        [Tooltip("Monster Prefab for start segments")]
        public int moncherChance;

        [Header("Door Prefabs")]

        public GameObject DoorWorking;
        public GameObject DoorClosed;
        public GameObject DoorWorking2;

        [Header("Door Options")]

        [Tooltip("Chance for a door to spawn each segment (higher = less)")]
        public int doorChance = 16;

        [Tooltip("Chance for a working door to spawn (higher = less)")]
        public int workingDoorChance = 6;

        [Tooltip("Chance for a \"Door Working\" to spawn instead of \"Door Working 2\" (higher = less)")]
        public int doorWorkingPrefabChance = 2;

        public override void OnSegmentEndGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {
            base.OnSegmentEndGenerated(behaviour, spawnPosition);

            if (Random.Range(0, doorChance) == 1)
            {
                if (Random.Range(0, workingDoorChance) == 0)
                {
                    if (Random.Range(0, doorWorkingPrefabChance) == 0)
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking, null, spawnPosition, lastSegmentEnd.rotation, null, true);
                    else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking2, null, spawnPosition, lastSegmentEnd.rotation, null, true);
                }
                else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorClosed, null, spawnPosition, lastSegmentEnd.rotation, null, true);
            }

            if (Settings.entities && Random.Range(0, moncherChance) == 1)
            {
                if (Random.Range(0, 350) == 1)
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabEnd, null, spawnPosition, new Quaternion(moncherPrefabEnd.transform.rotation.x, -moncherPrefabEnd.transform.rotation.y,
                        moncherPrefabEnd.transform.rotation.z, moncherPrefabEnd.transform.rotation.w), null, true);
                else
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabEnd, null, spawnPosition, moncherPrefabEnd.transform.rotation, null, true);
            }
        }

        public override void OnSegmentStartGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {
            base.OnSegmentStartGenerated(behaviour, spawnPosition);

            if (Random.Range(0, 16) == 1)
            {
                if (Random.Range(0, 6) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking, null, spawnPosition, lastSegmentEnd.rotation, null, true);
                    else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking2, null, spawnPosition, lastSegmentEnd.rotation, null, true);
                }
                else
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorClosed, null, spawnPosition, lastSegmentStart.rotation, null, true);
            }

            if (Settings.entities && Random.Range(0, moncherChance) == 1)
            {
                if (Random.Range(0, 350) == 1)
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabStart, null, spawnPosition, new Quaternion(moncherPrefabStart.transform.rotation.x, -moncherPrefabStart.transform.rotation.y,
                        moncherPrefabStart.transform.rotation.z, moncherPrefabStart.transform.rotation.w), null, true);
                else
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabStart, null, spawnPosition, moncherPrefabStart.transform.rotation, null, true);
            }
        }

        public override IEnumerator AwakeGen()
        {
            yield return base.AwakeGen();

            if (!ModNetworkManager.Instance.IsServer()) yield break;

            startSegmentOffset = -lastSegmentStart.right * 10f;
            endSegmentOffset = Vector3.zero;
        }
    }
}
