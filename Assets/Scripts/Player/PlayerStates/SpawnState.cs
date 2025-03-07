using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnState : State
{

    public SpawnState()
    {
        playerState = PlayerStateEnum.Spawn;
        animation = Animation.Idle;
        animationLabel = SpriteLabel.Default;
        animationDuration = 1f;
        animationLayer = 0;
        animationSpeed = 0f;
        animationHash = Animator.StringToHash(animation.ToString());
    }

}
