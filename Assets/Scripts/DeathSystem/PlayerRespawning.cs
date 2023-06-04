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
        _movementScript.CanPerformControllerMove = false;
        if (RespawnTracker.Instance.RespawnChangedSinceStart)
        {

            transform.position = RespawnTracker.Instance.RespawnPoint;
            transform.forward = RespawnTracker.Instance.RespawnForward;

            FindObjectOfType<CameraMovement>().transform.forward = RespawnTracker.Instance.RespawnForward;
        }
        StartCoroutine(ReEnableMovement());

    }

    private IEnumerator ReEnableMovement()
    {

        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        _movementScript.CanPerformControllerMove = true;
    }

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}


