using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBinder : MonoBehaviour
{

    [SerializeField] private GameObject[] playerBones;

    private Vector3[] spawnPositions;
    private Vector3[] topPositions;
    private Vector3[] targetPositions;
    private Vector3[] returnPositions;
    private Vector3[] attackPositions;


    private void Awake()
    {
        SaveOriginalPositions();
        targetPositions = spawnPositions;
        topPositions = spawnPositions;
        targetPositions = spawnPositions;
        returnPositions = spawnPositions;
        attackPositions = spawnPositions;
    }

    public Vector3[] GetSpawnPositions()
    {
        return spawnPositions;
    }

    public GameObject[] GetBones()
    {
        return playerBones;
    }

    public void SaveOriginalPositions()
    {
        for (int i = 0; i < playerBones.Length; i++)
        {
            spawnPositions[i] = playerBones[i].transform.position;
        }
    }

 }
