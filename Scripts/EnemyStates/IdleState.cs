using UnityEngine;

class IdleState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.MyTarget = null;
        this.parent.Patrol(true);

        // CALL RESET FUNCTION
        //this.parent.Reset();
    }


    public void Exit()
    {
        this.parent.Patrol(false);
    }

    public void Update()
    {
        //change into follow state if player is close
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());
        }
    }
}