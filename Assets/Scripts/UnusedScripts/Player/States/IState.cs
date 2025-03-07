using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void OnEnterState();

    public void ExecuteOnUpdate();

    public void ExecuteOnFixedUpdate();

    public void OnExitState();
}
