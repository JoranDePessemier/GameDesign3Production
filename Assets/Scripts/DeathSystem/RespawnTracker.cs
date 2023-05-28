using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTracker : MonoBehaviour
{
    
    public static RespawnTracker Instance;

    public  Vector3 RespawnPoint { get; set; }

    public Vector3 RespawnForward { get; set; }

    public bool RespawnChangedSinceStart { get; set; }

    private SpawnPoint _previousSpawn;

    internal void SetSpawn(SpawnPoint spawnObject, Transform spawn)
    {
        RespawnChangedSinceStart = true;
        RespawnPoint = spawn.position;
        RespawnForward = spawn.forward;

        if(spawnObject != _previousSpawn)
        {
            _previousSpawn?.SetAsInActive();
            _previousSpawn = spawnObject;
            _previousSpawn.SetAsActive();
        }


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
