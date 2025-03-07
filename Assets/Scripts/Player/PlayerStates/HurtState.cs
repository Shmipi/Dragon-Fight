using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : State
{
    public HurtState()
    {
        playerState = PlayerStateEnum.Hurt;
        animation = Animation.Hurt;
        animationLabel = SpriteLabel.Hurt;
        animationDuration = 1f / 3 ;
        animationLayer = 1;
        animationID = Animator.StringToHash("AnimHurtLayerSpeed");
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
