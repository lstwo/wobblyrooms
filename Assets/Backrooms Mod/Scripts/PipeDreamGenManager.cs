using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModWobblyLife;
using ModWobblyLife.Network;

public class PipeDreamGenManager : MonoBehaviour
{
    public GameObject hallwaySegmentPrefab;
    public Transform playerTransform;
    public int maxSegmentsInMemory = 3;
    public Transform premadeSegmentEnd;
    public Transform premadeSegmentStart;

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
            GameObject newSegment = Instantiate(hallwaySegmentPrefab, spawnPosition, lastSegmentEnd.rotation, transform);
            lastSegmentEnd = newSegment.transform.Find("End");
        } else
        {
            Vector3 spawnPosition = lastSegmentStart.position + lastSegmentStart.right * 10;
            GameObject newSegment = Instantiate(hallwaySegmentPrefab, spawnPosition, lastSegmentEnd.rotation, transform);
            lastSegmentStart = newSegment.transform.Find("Start");
        }
        
    }



    private void ManageHallwaySegments()
    {
        // Check if lastSegmentEnd and playerTransform are not null before proceeding.
        if (lastSegmentEnd != null && playerTransform != null && hallwaySegmentPrefab != null)
        {

            float distanceToLastSegment = Vector3.Distance(playerTransform.position, lastSegmentEnd.position);

            Renderer segmentRenderer = hallwaySegmentPrefab.transform.Find("Segment").GetComponent<Renderer>();
            if (segmentRenderer != null && distanceToLastSegment <= maxSegmentsInMemory * 10)
            {
                GenerateNewSegment(true);
            }

            float distanceToLastSegmentStart = Vector3.Distance(playerTransform.position, lastSegmentStart.position * 10);
            if (segmentRenderer != null && distanceToLastSegment >= maxSegmentsInMemory * 10)
            {
                GenerateNewSegment(false);
            }

            // Unload segments that are too far from the player.
            Transform[] segments = GetComponentsInChildren<Transform>();
            foreach (Transform segment in segments)
            {
                if (segment != null && segment.Find("Segment") != null)
                {

                    Renderer _segmentRenderer = segment.Find("Segment").GetComponent<Renderer>();
                    if (segmentRenderer != null)
                    {
                        float distanceToSegment = Vector3.Distance(playerTransform.position, segment.position);
                        if (distanceToSegment > maxSegmentsInMemory * _segmentRenderer.bounds.size.x)
                        {
                            Destroy(segment.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void GenerateNewHallwayOnInteraction()
    {
    }

}
