using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpState : State
{
    public MoveUpState()
    {
        playerState = PlayerStateEnum.MoveUp;
        animation = Animation.MoveUp;
        animationLabel = SpriteLabel.OpenMouth;
        animationDuration = 0.85f;
        animationSpeed = 1f / animationDuration;
        animationLayer = 0;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
