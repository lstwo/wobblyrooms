using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

public class Level4Generator : MonoBehaviour
{
    public static List<Transform> playerTransforms = new List<Transform>();
    public static ModPlayerController[] playerControllers;

    public GameObject hallwaySegmentPrefab;
    public int maxSegmentsInMemory = 10;
    public Transform premadeSegmentEnd;
    public Transform premadeSegmentStart;
    public GameObject[] interiors;

    public GameObject moncherPrefabEnd;
    public GameObject moncherPrefabStart;
    public int moncherChance;

    public GameObject DoorWorking, DoorClosed;

    private Transform lastSegmentEnd;
    private Transform lastSegmentStart;

    bool done = true;

    private void Start()
    {
        //playerTransform = transform;
        playerTransforms.Clear();
        playerControllers = ModInstance.Instance.GetModPlayerControllers();
        foreach (ModPlayerController controller in playerControllers) playerTransforms.Add(controller.GetPlayerTransform());

        lastSegmentEnd = premadeSegmentEnd;
        lastSegmentStart = premadeSegmentStart;

        StartCoroutine(StartGenerate());
    }

    private void Update()
    {
        playerTransforms.Clear();
        playerControllers = ModInstance.Instance.GetModPlayerControllers();
        foreach (ModPlayerController controller in playerControllers) playerTransforms.Add(controller.GetPlayerTransform());
        ManageHallwaySegments();
    }

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

    private void GenerateNewSegment(bool end)
    {
        done = false;

        if(end)
        {
            Vector3 spawnPosition = lastSegmentEnd.position;
            ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) =>
            {
                go.transform.SetParent(transform.Find("ForwardsSpawner"));

                if (Random.Range(0, 2) == 1)
                {
                    int rand = Random.Range(0, interiors.Length);
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(interiors[rand], null, spawnPosition, lastSegmentEnd.rotation, null, true);
                }

                lastSegmentEnd = go.transform.Find("End");

                done = true;
            }, spawnPosition, lastSegmentEnd.rotation, null, true);
        }
        else
        {
            Vector3 spawnPosition = lastSegmentStart.position + -lastSegmentStart.up * 8;
            ModNetworkManager.Instance.InstantiateNetworkPrefab(hallwaySegmentPrefab, (go) =>
            {
                go.transform.SetParent(transform.Find("BackwardsSpawner"));

                if (Random.Range(0, 2) == 1)
                {
                    int rand = Random.Range(0, interiors.Length);
                    ModNetworkManager.Instance.InstantiateNetworkPrefab(interiors[rand], null, spawnPosition, lastSegmentStart.rotation, null, true);
                }

                lastSegmentStart = go.transform.Find("Start");

                done = true;
            }, spawnPosition, lastSegmentStart.rotation, null, true);
        }
    }



    private void ManageHallwaySegments()
    {
        foreach(Transform playerTransform in playerTransforms)
        {
            if (!done) return;

            if (lastSegmentEnd != null && playerTransform != null && hallwaySegmentPrefab != null)
            {
                float distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentEnd.position);

                if (distanceToLastSegment <= maxSegmentsInMemory * 8)
                {
                    GenerateNewSegment(true);
                }

                distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentStart.position);
                if (distanceToLastSegment <= maxSegmentsInMemory * 8)
                {
                    GenerateNewSegment(false);
                }
            }
        }
    }

    public void GenerateNewHallwayOnInteraction()
    {
    }

}
