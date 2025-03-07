using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownState : State
{
    public MoveDownState()
    {
        playerState = PlayerStateEnum.MoveDown;
        animation = Animation.MoveDown;
        animationLabel = SpriteLabel.CloseMouth;
        animationDuration = 1.25f;
        animationSpeed = 1.5f / animationDuration;
        animationLayer = 0;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
