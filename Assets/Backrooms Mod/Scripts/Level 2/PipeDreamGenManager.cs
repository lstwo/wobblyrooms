using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

public class PipeDreamGenManager : MonoBehaviour
{
    public GameObject hallwaySegmentPrefab;
    public Transform playerTransform;
    public int maxSegmentsInMemory = 10;
    public Transform premadeSegmentEnd;
    public Transform premadeSegmentStart;

    public GameObject moncherPrefabEnd;
    public GameObject moncherPrefabStart;
    public int moncherChance;

    public GameObject DoorWorking, DoorClosed, DoorWorking2;

    private Transform lastSegmentEnd;
    private Transform lastSegmentStart;

    private void Start()
    {
        //playerTransform = transform;
        playerTransform = Gamemode.instance.playerTransform;
        lastSegmentEnd = premadeSegmentEnd;
        lastSegmentStart = premadeSegmentStart;
        GenerateHallwaySegments(maxSegmentsInMemory);
    }

    private void Update()
    {
        playerTransform = Gamemode.instance.playerTransform;
        ManageHallwaySegments();
    }

    private void GenerateHallwaySegments(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GenerateNewSegment(true);
        }
        for (int i = 0; i < count; i++)
        {
            GenerateNewSegment(false);
        }
    }

    private void GenerateNewSegment(bool end)
    {
        if(end)
        {
            Vector3 spawnPosition = lastSegmentEnd.position;
            GameObject newSegment = Instantiate(hallwaySegmentPrefab, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
            lastSegmentEnd = newSegment.transform.Find("End");
            if(Random.Range(0, 150) == 1)
            {
                if (Random.Range(0, 4) == 0)
                {
                    if(Random.Range(0, 2) == 0)
                        Instantiate(DoorWorking, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
                    else Instantiate(DoorWorking2, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
                }
                else Instantiate(DoorClosed, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
            }

            if (Random.Range(0, moncherChance) == 1)
            {
                if(Random.Range(0, 350) == 1)
                    Instantiate(moncherPrefabEnd, spawnPosition, new Quaternion(moncherPrefabEnd.transform.rotation.x, -moncherPrefabEnd.transform.rotation.y,
                        moncherPrefabEnd.transform.rotation.z, moncherPrefabEnd.transform.rotation.w));
                else
                    Instantiate(moncherPrefabEnd, spawnPosition, moncherPrefabEnd.transform.rotation);
            }
        }
        else
        {
            Vector3 spawnPosition = lastSegmentStart.position + -lastSegmentStart.right * 10;
            GameObject newSegment = Instantiate(hallwaySegmentPrefab, spawnPosition, lastSegmentStart.rotation, transform.Find("BackwardsSpawner"));
            lastSegmentStart = newSegment.transform.Find("Start");
            if (Random.Range(0, 150) == 1)
            {
                if (Random.Range(0, 4) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                        Instantiate(DoorWorking, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
                    else Instantiate(DoorWorking2, spawnPosition, lastSegmentEnd.rotation, transform.Find("ForwardsSpawner"));
                }
                else
                    Instantiate(DoorClosed, spawnPosition, lastSegmentStart.rotation, transform.Find("BackwardsSpawner"));
            }

            if (Random.Range(0, moncherChance) == 1)
            {
                if (Random.Range(0, 350) == 1)
                    Instantiate(moncherPrefabStart, spawnPosition, new Quaternion(moncherPrefabStart.transform.rotation.x, -moncherPrefabStart.transform.rotation.y,
                        moncherPrefabStart.transform.rotation.z, moncherPrefabStart.transform.rotation.w));
                else
                    Instantiate(moncherPrefabStart, spawnPosition, moncherPrefabStart.transform.rotation);
            }
        }
    }



    private void ManageHallwaySegments()
    {
        // Check if lastSegmentEnd and playerTransform are not null before proceeding.
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

    public void GenerateNewHallwayOnInteraction()
    {
    }

}
