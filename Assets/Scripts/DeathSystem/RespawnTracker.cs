using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTracker : MonoBehaviour
{
    
    public static RespawnTracker Instance;

    public  Vector3 RespawnPoint { get; set; }

    public bool RespawnChangedSinceStart { get; set; }

    private SpawnPoint _previousSpawn;

    internal void SetSpawn(SpawnPoint spawnObject, Vector3 spawn)
    {
        RespawnChangedSinceStart = true;
        RespawnPoint = spawn;
        _previousSpawn?.SetAsInActive();
        _previousSpawn = spawnObject;
        _previousSpawn.SetAsActive();

    }

    private void Awake()
    {
        if (RespawnTracker.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(RespawnPoint.ToString());
    }


}
