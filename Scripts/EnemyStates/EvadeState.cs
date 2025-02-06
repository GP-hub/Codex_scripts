using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvadeState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;

        if (parent.MyTarget != null)
        {
            parent.MyTarget.RemoveAttacker(parent);
        }
    }

    public void Exit()
    {
        parent.Direction = Vector3.zero;
        //parent.Reset();
    }

    public void Update()
    {
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;
        parent.transform.position = Vector3.MoveTowards
            (parent.transform.position, parent.MyStartPosition, parent.MyNavMeshAgent.speed * Time.deltaTime);
        float distance = Vector3.Distance(parent.MyStartPosition, parent.transform.position);

        if (distance <= 0)
        {
            Debug.Log("check 2");
            parent.ChangeState(new IdleState());
        }
    
    }
}
