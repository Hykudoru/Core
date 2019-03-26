using System;
using System.Collections.Generic;
using UnityEngine;

public enum Status : byte
{
    Incomplete = 0,
    Complete = 1
}

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
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
        private set { enemyCount = value; }
    }

    public Spawner[] Spawners
    {
        get { return spawners; }
        private set { spawners = value; }
    }

    public Transform[] SpawnPoints
    {
        get { return spawnPoints; }
        private set { spawnPoints = value; }
    }

    public float TimeLimit
    {
        get { return timeLimit; }
        private set { timeLimit = value; }
    }

    public bool IsTimed
    {
        get { return isTimed; }
        private set { isTimed = value; }
    }
}