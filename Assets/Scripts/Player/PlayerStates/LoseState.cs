using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : State
{
    public LoseState()
    {
        playerState = PlayerStateEnum.Lose;
        animation = Animation.Lose;
        animationLabel = SpriteLabel.Defeated;
        animationDuration = 1f;
        animationLayer = 0;
        loopAnimation = true;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
