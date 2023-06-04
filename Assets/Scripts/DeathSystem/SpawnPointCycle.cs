using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPointCycle : MonoBehaviour
{
    PlayerRespawning _respawnScript;

    [SerializeField]
    private SpawnPoint[] _points;

    private int _currentSpawnIndex = 0;

    private Controls _controls;

    private void Awake()
    {
        foreach (var point in _points)
        {
            point.CheckPointActivated += SpawnPoint_CheckPointActivated;
        }

        _controls = new Controls();
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.PlayerControls.Debug_NextLevel.performed += NextLevel;
        _controls.PlayerControls.Debug_PreviousLevel.performed += PreviousLevel;
    }



    private void OnDisable()
    {
        _controls.Disable();
        _controls.PlayerControls.Debug_NextLevel.performed -= NextLevel;
        _controls.PlayerControls.Debug_PreviousLevel.performed -= PreviousLevel;
    }



    private void Start()
    {
        _respawnScript = FindObjectOfType<PlayerRespawning>();
    }

    private void SpawnPoint_CheckPointActivated(object sender, EventArgs e)
    {
        SpawnPoint activated = sender.ConvertTo<SpawnPoint>();

        _currentSpawnIndex = Array.IndexOf(_points, activated);
    }
    private void PreviousLevel(InputAction.CallbackContext obj)
    {
        if(_currentSpawnIndex > 0)
        {
            SetNewSpawn(_currentSpawnIndex - 1);
        }
    }



    private void NextLevel(InputAction.CallbackContext obj)
    {
        if (_currentSpawnIndex < _points.Length - 1)
        {
            SetNewSpawn(_currentSpawnIndex + 1);
        }
    }

    private void SetNewSpawn(int index)
    {
        SpawnPoint spawnPoint = _points[index];
        RespawnTracker.Instance.SetSpawn(spawnPoint, spawnPoint.SpawnPosition);
        _respawnScript.Respawn();
    }

}
