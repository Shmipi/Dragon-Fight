using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    public void MoveOnHold()
    {
        playerBehaviour.MoveOnHoldButton();
    }

    public void MoveOnRelease()
    {
        playerBehaviour.MoveOnReleaseButton();
    }
}
