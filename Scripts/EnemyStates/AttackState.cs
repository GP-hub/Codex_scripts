using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private int oldAvoidancePriority;

    //private float extraRange = .1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.MyNavMeshAgent.isStopped = true;
        oldAvoidancePriority = parent.MyNavMeshAgent.avoidancePriority;
        parent.MyNavMeshAgent.avoidancePriority = 1;
    }

    public void Exit()
    {
        // TEST
        //parent.MyNavMeshAgent.ResetPath();
        parent.MyNavMeshAgent.avoidancePriority = oldAvoidancePriority;
    }

    public void Update()
    {
        if (parent.MyAttackTime >= parent.MyAttackCooldown && !parent.IsAttacking && parent.CanSeePlayer())
        {
            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        if (parent.MyTarget != null)
        {
            
            parent.transform.LookAt(parent.MyTarget.transform);

            float distance = Vector3.Distance(parent.MyTarget.transform.position, parent.transform.position);

            if (distance >= parent.MyAttackRange + parent.MyAttackExtraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());

                parent.MyNavMeshAgent.isStopped = false;
            }
            // We need to check range and attack
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        if (parent.MyTrailWeapon != null)
        {
            parent.MyTrailWeapon.SetActive(true);
        }

        if (parent.MyType == "Boss")
        {
            bool inRange = parent.GetComponent<BossEnemy>().PickMove();

            if (inRange)
            {
                // In range
            }
            if (!inRange)
            {
                parent.IsAttacking = false;
                parent.ChangeState(new FollowState());
                yield break;
            }
        }
        parent.MyAnimator.SetTrigger("attack");
        parent.MyAnimator.Play("wait", 2);

        //wait for the length of the anim to set bool to false
        //yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length * parent.MyAnimator.GetCurrentAnimatorStateInfo(2).speed);
        yield return new WaitForSeconds(parent.MyAttackAnimation.length);

        parent.IsAttacking = false;

        if (parent.MyTrailWeapon != null)
        {
            parent.MyTrailWeapon.SetActive(false);
        }

        parent.MyAnimator.ResetTrigger("attack");
    }
}