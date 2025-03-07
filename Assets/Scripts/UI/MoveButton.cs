using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    private PlayerBehaviour playerBehaviour;

    public void MoveOnHold()
    {
        playerBehaviour.MoveOnHoldButton();
    }

    public void MoveOnRelease()
    {
        playerBehaviour.MoveOnReleaseButton();
    }

    public void SetPlayer(PlayerBehaviour playerBehaviour)
    {
        this.playerBehaviour = playerBehaviour;
    }
}
