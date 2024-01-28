using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;
using System;
using System.Transactions;

public class PipeDreamGenManager : MonoBehaviour
{
    public GameObject hallwaySegmentPrefab;
    public static List<Transform> playerTransforms = new List<Transform>();
    public static ModPlayerController[] playerControllers;
    public int maxSegmentsInMemory = 10;
    public Transform premadeSegmentEnd;
    public Transform premadeSegmentStart;

    public GameObject moncherPrefabEnd;
    public GameObject moncherPrefabStart;
    public int moncherChance;

    public GameObject DoorWorking, DoorClosed, DoorWorking2;

    private Transform lastSegmentEnd;
    private Transform lastSegmentStart;

    private bool end = false;
    private bool done = true;

    private void Start()
    {
        if (!NetworkManager.instance.IsServer()) return;

        playerTransforms.Clear();
        playerControllers = ModInstance.Instance.GetModPlayerControllers();
        foreach(ModPlayerController controller in playerControllers) playerTransforms.Add(controller.GetPlayerTransform());

        StartCoroutine(StartGenerate());
    }

    private void Update()
    {
        if (!NetworkManager.instance.IsServer()) return;

        playerTransforms.Clear();
        playerControllers = ModInstance.Instance.GetModPlayerControllers();
        foreach (ModPlayerController controller in playerControllers) playerTransforms.Add(controller.GetPlayerTransform());

        ManageHallwaySegments();
    }

    private IEnumerator StartGenerate()
    {
        for (int i = 0; i < maxSegmentsInMemory; i++)
        {
            while (!done) yield return null;
            yield return null;
            StartCoroutine(GenerateNewSegment(true));
            while (!done) yield return null;
            yield return null;
            StartCoroutine(GenerateNewSegment(false));
        }
    }

    private IEnumerator GenerateNewSegment(bool _end)
    {
        while(!done)
            yield return null;

        Debug.Log("Done and Continuing...");

        done = false;
        end = _end;

        if(end)
        {
            Debug.Log("End: Generating...");

            Vector3 spawnPosition = lastSegmentEnd.position;
            ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) =>
            {
                Debug.Log("End: Generated and Continuing...");

                go.transform.SetParent(transform.Find("ForwardsSpawner"));

                lastSegmentEnd = go.transform.Find("End");
                if (UnityEngine.Random.Range(0, 150) == 1)
                {
                    if (UnityEngine.Random.Range(0, 4) == 0)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                            ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking, null, spawnPosition, lastSegmentEnd.rotation);
                        else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking2, null, spawnPosition, lastSegmentEnd.rotation);
                    }
                    else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorClosed, null, spawnPosition, lastSegmentEnd.rotation);
                }

                if (UnityEngine.Random.Range(0, moncherChance) == 1)
                {
                    if (UnityEngine.Random.Range(0, 350) == 1)
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabEnd, null, spawnPosition, new Quaternion(moncherPrefabEnd.transform.rotation.x, -moncherPrefabEnd.transform.rotation.y,
                            moncherPrefabEnd.transform.rotation.z, moncherPrefabEnd.transform.rotation.w), null, true);
                    else
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabEnd, null, spawnPosition, moncherPrefabEnd.transform.rotation);
                }

                Debug.Log("End: Finished!");

                done = true;

            }, spawnPosition, lastSegmentEnd.rotation, null, false);
        } 
        else
        {
            Debug.Log("Start: Generating...");

            Vector3 spawnPosition = lastSegmentStart.position + -lastSegmentStart.right * 10;
            ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) => 
            {
                Debug.Log("Start: Generated and Continuing...");

                go.transform.SetParent(transform.Find("BackwardsSpawner"));

                lastSegmentStart = go.transform.Find("Start");
                if (UnityEngine.Random.Range(0, 150) == 1)
                {
                    if (UnityEngine.Random.Range(0, 4) == 0)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                            ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking, null, spawnPosition, lastSegmentEnd.rotation);
                        else ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorWorking2, null, spawnPosition, lastSegmentEnd.rotation);
                    }
                    else
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(DoorClosed, null, spawnPosition, lastSegmentStart.rotation);
                }

                if (UnityEngine.Random.Range(0, moncherChance) == 1)
                {
                    if (UnityEngine.Random.Range(0, 350) == 1)
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabStart, null, spawnPosition, new Quaternion(moncherPrefabStart.transform.rotation.x, -moncherPrefabStart.transform.rotation.y,
                            moncherPrefabStart.transform.rotation.z, moncherPrefabStart.transform.rotation.w));
                    else
                        ModNetworkManager.Instance.InstantiateNetworkPrefab(moncherPrefabStart, null, spawnPosition, moncherPrefabStart.transform.rotation);
                }

                Debug.Log("End: Finished!");

                done = true;

            }, spawnPosition, lastSegmentStart.rotation, null, false);
        }

    }

    private void ManageHallwaySegments()
    {
        foreach(Transform playerTransform in playerTransforms)
        {
            if (lastSegmentEnd != null && playerTransform != null && hallwaySegmentPrefab != null)
            {
                float distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentEnd.position);

                if (distanceToLastSegment <= maxSegmentsInMemory * 10)
                {
                    StartCoroutine(GenerateNewSegment(true));
                }

                distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentStart.position);
                if (distanceToLastSegment <= maxSegmentsInMemory * 10)
                {
                    StartCoroutine(GenerateNewSegment(false));
                }
            }
        }
    }
}
