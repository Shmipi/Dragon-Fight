using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState()
    {
        playerState = PlayerStateEnum.Idle;
        animation = Animation.Idle;
        animationLabel = SpriteLabel.Default;
        animationDuration = 3f;
        animationLayer = 0;
        loopAnimation = true;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
