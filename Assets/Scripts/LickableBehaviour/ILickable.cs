using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILickable
{
     bool IsEatable { get; protected set; }
    void HoldingLicked(Transform playerTransform);

    void Licked(Transform playerTransform);
    bool LickedReleased(Transform playerTransform);
}
