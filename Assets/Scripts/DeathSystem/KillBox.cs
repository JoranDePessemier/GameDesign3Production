using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IRespawn
{
    public void Respawn();
}

public class KillBox : MonoBehaviour, ICharacterHit
{
    public void CharacterHit(GameObject character)
    {
        ExecuteKill(character);
    }

    public void OnCollisionEnter(Collision collision)
    {
        ExecuteKill(collision.gameObject);
    }

    private void ExecuteKill(GameObject collisionObject)
    {
        if(collisionObject.TryGetComponent<IRespawn>(out IRespawn respawn))
        {
            respawn.Respawn();
        }
        else
        {
            collisionObject.SetActive(false);
        }
    }
}
