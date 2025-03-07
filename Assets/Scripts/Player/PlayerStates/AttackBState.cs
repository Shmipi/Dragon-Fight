using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBState : State
{
    public AttackBState()
    {
        playerState = PlayerStateEnum.AttackB;
        animation = Animation.Attack;
        animationLabel = SpriteLabel.CloseMouth;
        animationDuration = 1f;
        animationLayer = 2;
        animationID =  Animator.StringToHash("AnimAttackLayerSpeed");
        animationSpeed = -1f / animationDuration;
        animationHash = Animator.StringToHash(animation.ToString());
    }
}
