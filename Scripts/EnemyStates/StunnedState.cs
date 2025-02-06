using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StunnedState : IState
{
    public void Enter(Enemy parent)
    {
        parent.MyNavMeshAgent.isStopped = true;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
