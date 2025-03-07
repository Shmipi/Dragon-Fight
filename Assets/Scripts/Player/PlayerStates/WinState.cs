using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : State
{
    public WinState()
    {
        playerState = PlayerStateEnum.Win;
        animation = Animation.Win;
        animationLabel = SpriteLabel.Default;
        animationDuration = 1f;
        animationLayer = 0;
        loopAnimation = true;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
