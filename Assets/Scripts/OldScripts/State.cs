using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{

    protected BattleSystem battleSystem;

    public State(BattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual void StandBy()
    {

    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void GetHurt()
    {

    }



}
