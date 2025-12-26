using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform streetContainer;
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private int segmentsOnScreen = 5;
    [SerializeField] private float segmentLength = 70.0f;

    private float nextSegmentPositionX = 0;
    private bool playerEnteredStreet = false;
    private LinkedList<GameObject> activeSegments = new LinkedList<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
        if (player == null)
        {
            Debug.LogError("Player not found");
        }

        if (streetPrefab == null)
        {
            Debug.LogError("Street prefab not assigned");
        }

        for (int i = 0; i < segmentsOnScreen; i++)
        {
            Vector3 spawnPosition = new Vector3(nextSegmentPositionX, 0, 0);
            GameObject spawnedSegment = Instantiate(streetPrefab, spawnPosition, Quaternion.identity, streetContainer);
            activeSegments.AddLast(spawnedSegment);
            nextSegmentPositionX += segmentLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || activeSegments.Count == 0) return;

        if (player.position.x > segmentLength / 2 && player.position.z < 2.0f && player.position.z > -2.0f)
        {
            playerEnteredStreet = true;
        }
        if (playerEnteredStreet) SpawnStreetSegment();
    }

    private void SpawnStreetSegment()
    {
        if (player.position.x > activeSegments.Last.Value.transform.position.x - segmentLength / 2)
        {
            GameObject firstSegment = activeSegments.First.Value;
            activeSegments.RemoveFirst();
            Vector3 spawnPos = activeSegments.Last.Value.transform.position + Vector3.right * segmentLength;
            firstSegment.transform.position = spawnPos;
            activeSegments.AddLast(firstSegment);
        }

        if (player.position.x < activeSegments.First.Value.transform.position.x + segmentLength / 2)
        {
            GameObject lastSegment = activeSegments.Last.Value;
            activeSegments.RemoveLast();
            Vector3 spawnPos = activeSegments.First.Value.transform.position - Vector3.right * segmentLength;
            lastSegment.transform.position = spawnPos;
            activeSegments.AddFirst(lastSegment);
        }
    }
}