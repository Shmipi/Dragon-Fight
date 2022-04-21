using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : StateMachine
{
    private void Start()
    {
        SetState(new StartState(this));
        StartCoroutine(state.Start());
    }

    public void OnAttackButton()
    {
        state.Attack();
    }

    public void OnMoveButton()
    {
        state.Move();
    }

    private void Update()
    {
        
    }


    private void FixedUpdate()
    {
       
    }
}
