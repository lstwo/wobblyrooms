using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

public class Level4Generator : MonoBehaviour
{
    public GameObject hallwaySegmentPrefab;
    public Transform playerTransform;
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

    private void Start()
    {
        //playerTransform = transform;
        if(Gamemode.instance != null && Gamemode.instance.playerTransform != null)
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
            if(Random.Range(0, 2) == 1)
            {
                int rand = Random.Range(0, interiors.Length);
                Instantiate(interiors[rand], spawnPosition, lastSegmentEnd.rotation, newSegment.transform);
            }

            lastSegmentEnd = newSegment.transform.Find("End");
        }
        else
        {
            Vector3 spawnPosition = lastSegmentStart.position + -lastSegmentStart.up * 8;
            GameObject newSegment = Instantiate(hallwaySegmentPrefab, spawnPosition, lastSegmentStart.rotation, transform.Find("BackwardsSpawner"));
            if (Random.Range(0, 2) == 1)
            {
                int rand = Random.Range(0, interiors.Length);
                Instantiate(interiors[rand], spawnPosition, lastSegmentStart.rotation, newSegment.transform);
            }

            lastSegmentStart = newSegment.transform.Find("Start");
        }
    }



    private void ManageHallwaySegments()
    {
        // Check if lastSegmentEnd and playerTransform are not null before proceeding.
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

    public void GenerateNewHallwayOnInteraction()
    {
    }

}
