using System;
using System.Collections.Generic;
using UnityEngine;

public enum Status : byte
{
    Incomplete = 0,
    Complete = 1
}

//[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
[Serializable]
public class Wave : ScriptableObject
{
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] float timeLimit = 0.0f;
    [SerializeField] bool isTimed = false;

    public int EnemyCount
    {
        get { return enemyCount; }
    }

    public Spawner[] Spawners
    {
        get { return spawners; }
    }

    public Transform[] SpawnPoints
    {
        get { return spawnPoints; }
    }

    public float TimeLimit
    {
        get { return timeLimit; }
    }

    public bool IsTimed
    {
        get { return isTimed; }
    }
}