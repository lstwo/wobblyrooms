using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

namespace Wobblyrooms
{
    public class HallwayGenManager : BaseGenerationManager
    {
        [Header("Hallway Segments")]

        [Tooltip("Prefab to spawn as hallway segments")]
        public GameObject hallwaySegmentPrefab;

        [Tooltip("The end transform of your premade segment(s)")]
        public Transform premadeSegmentEnd;

        [Tooltip("The start transform of your premade segment(s)")]
        public Transform premadeSegmentStart;

        [Tooltip("Distance between the player and the next segment to generate in segments")]
        public int maxSegmentsInMemory = 10;

        [Tooltip("DONT SET IF IT ISNT SET TO 0,0,0 BY DEFAULT")]
        public Vector3 startSegmentOffset;

        [Tooltip("DONT SET IF IT ISNT SET TO 0,0,0 BY DEFAULT")]
        // IMPORTANT: Set to -10 for level 2 (-lastSegmentStart.right * 10f)
        public Vector3 endSegmentOffset;

        protected Transform lastSegmentEnd;
        protected Transform lastSegmentStart;

        protected bool end = false;
        protected bool done = true;

        private IEnumerator StartGenerate()
        {
            for (int i = 0; i < maxSegmentsInMemory; i++)
            {
                GenerateNewSegment(true);
                while (!done) yield return null;
                GenerateNewSegment(false);
                while (!done) yield return null;
            }
        }

        private void GenerateNewSegment(bool _end)
        {
            done = false;
            end = _end;

            if (end)
            {
                Vector3 spawnPosition = lastSegmentEnd.position + endSegmentOffset;
                ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) =>
                {
                    go.transform.SetParent(transform.Find("ForwardsSpawner"));

                    lastSegmentEnd = go.transform.Find("End");

                    OnSegmentEndGenerated(go, spawnPosition);

                    done = true;

                }, spawnPosition, lastSegmentEnd.rotation, null, true);
            }
            else
            {
                Vector3 spawnPosition = lastSegmentStart.position + startSegmentOffset;
                ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) =>
                {
                    go.transform.SetParent(transform.Find("BackwardsSpawner"));

                    lastSegmentStart = go.transform.Find("Start");

                    OnSegmentStartGenerated(go, spawnPosition);

                    done = true;

                }, spawnPosition, lastSegmentStart.rotation, null, true);
            }

        }

        private void ManageHallwaySegments()
        {
            foreach (Transform playerTransform in GameMode.Instance.playerTransforms)
            {
                if (!done) return;

                if (lastSegmentEnd != null && playerTransform != null && hallwaySegmentPrefab != null)
                {
                    float distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentEnd.position);

                    if (distanceToLastSegment <= maxSegmentsInMemory * 10)
                    {
                        GenerateNewSegment(true);
                    }

                    distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentStart.position);
                    if (distanceToLastSegment <= maxSegmentsInMemory * 10)
                    {
                        GenerateNewSegment(false);
                    }
                }
            }
        }

        public override IEnumerator AwakeGen()
        {
            lastSegmentEnd = premadeSegmentEnd;
            lastSegmentStart = premadeSegmentStart;

            yield break;
        }

        public override IEnumerator StartGen()
        {
            if (!ModNetworkManager.Instance.IsServer()) yield break;

            StartCoroutine(StartGenerate());
        }

        public override IEnumerator UpdateGen(List<ModPlayerController> controllers)
        {
            if (!ModNetworkManager.Instance.IsServer()) yield break;

            ManageHallwaySegments();
            yield break;
        }

        public override void EndGen()
        {
            return;
        }

        /// <summary>
        /// Called when a start segment was generated. Use this to spawn extra stuff for the segment
        /// </summary>
        /// <param name="behaviour"></param>
        public virtual void OnSegmentStartGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {

        }

        /// <summary>
        /// Called when an end segment was generated. Use this to spawn extra stuff for the segment
        /// </summary>
        /// <param name="behaviour"></param>
        public virtual void OnSegmentEndGenerated(ModNetworkBehaviour behaviour, Vector3 spawnPosition)
        {

        }
    }
}