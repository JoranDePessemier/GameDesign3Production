using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterHit
{
    public void CharacterHit(GameObject character);
}

public class CharacterCollision : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.TryGetComponent<ICharacterHit>(out ICharacterHit characterHit))
        {
            characterHit.CharacterHit(this.gameObject);
        }
    }
}
