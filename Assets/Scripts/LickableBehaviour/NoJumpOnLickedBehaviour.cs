using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJumpOnLickedBehaviour : MonoBehaviour
{
    private LickEatable _eatableScript;

    private CharacterMovement _charMovementScript;
    private void Awake()
    {
        _eatableScript = this.GetComponent<LickEatable>();
        _eatableScript.Eaten += ObjectEaten;
        _eatableScript.Released += ObjectReleased;
    }

    private void ObjectReleased(object sender, EventArgs e)
    {
        _charMovementScript.CanJump = true;
    }

    private void ObjectEaten(object sender, EventArgs e)
    {
        _charMovementScript.CanJump = false;
    }

    private void Start()
    {
        _charMovementScript = FindObjectOfType<CharacterMovement>();
    }

}
