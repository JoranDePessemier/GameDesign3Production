using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawning : MonoBehaviour, IRespawn
{
    private CharacterMovement _movementScript;

    private void Awake()
    {
        _movementScript = this.GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        _movementScript.CanMove = false;
        if (RespawnTracker.Instance.RespawnChangedSinceStart)
        {
            transform.position = RespawnTracker.Instance.RespawnPoint;
        }
        StartCoroutine(ReEnableMovement());

    }

    private IEnumerator ReEnableMovement()
    {

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        _movementScript.CanMove = true;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

