using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class WaitState : IState
{
    private Enemy parent;
    private Transform closestEnemy;
    public static List<Collider> enemies = new List<Collider>();
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.transform.LookAt(parent.MyTarget.transform);
        // CALL RESET FUNCTION
        //this.parent.Reset();

        //parent.MyNavMeshAgent.SetDestination(parent.MyTarget.transform.position + Random.insideUnitSphere * 1);
        parent.MyNavMeshAgent.isStopped = true;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MyTarget != null && parent.CanSeePlayer())
        {
            float distance = Vector3.Distance(parent.MyTarget.transform.position, parent.transform.position);

            parent.transform.LookAt(parent.MyTarget.transform);

            LayerMask mask = LayerMask.GetMask("Clickable");
            Collider[] tmp = Physics.OverlapCapsule(parent.MyTarget.transform.position, parent.MyTarget.transform.position, 2, mask);
            Collider[] tmpBox = Physics.OverlapBox(parent.MyNavMeshAgent.transform.position + (parent.MyTarget.transform.position - parent.MyNavMeshAgent.transform.position) * 0.5f, new Vector3(.5f, .5f, distance * .5f), parent.transform.rotation, mask);
            // test frontal collision
            //Collider[] tmpBox = Physics.OverlapBox(parent.MyNavMeshAgent.transform.position, new Vector3(1, 1, 1), parent.transform.rotation, mask);


            //Debug.Log(parent.name + " : wait state");
            //Debug.Log(parent.name + " : tmpBox contains : " + tmpBox.Length + " entities");

            //MAYBE GOING TO FOLLOW A DIFFERENT POSITION AROUND THE PLAYER ???
            if (tmpBox.Length == 1) 
            {
                //Debug.Log(parent.name + " : tmpBox contrains only 1 entity");
                parent.ChangeState(new FollowState());                
            }

            // faire la distance entre toutes les entités présent dans la box
            // si la distance est supérieur a XX, repasser en follow, jusqu'a être a nouveau bloqué


            enemies.Clear();
            enemies.AddRange(tmpBox);
            closestEnemy = parent.GetClosestEnemy(enemies, parent.transform);
            //Debug.Log(parent.name + " closest enemy is " + closestEnemy);
            //CALCULATE THE DISTANCE TO CLOSEST ENEMY & DO SMTH ABOUT IT
            //Debug.Log((closestEnemy.position - parent.transform.position).sqrMagnitude);

            //Debug.Log(closestEnemy.position);
            //Debug.Log(parent.MyTarget.transform.position);
            //Debug.Log(closestEnemy.position);

            if (closestEnemy !=null)
            {
                if ((closestEnemy.position - parent.MyTarget.transform.position).sqrMagnitude > (parent.transform.position - parent.MyTarget.transform.position).sqrMagnitude)
                {
                    //Debug.Log(parent.name + " : i'm closer to the target");
                    parent.ChangeState(new FollowState());
                }

                if ((closestEnemy.position - parent.transform.position).sqrMagnitude > 3f)
                {
                    //Debug.Log(parent.name + " : back to following cause the closest ally is a bit far");
                    parent.ChangeState(new FollowState());
                }
            }

            //if (tmp.Length <= 5)
            //{
            //    parent.ChangeState(new FollowState());
            //}

            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        //if (!parent.InRange)
        //{
        //    parent.ChangeState(new IdleState());
        //}
        else if (!parent.CanSeePlayer())
        {
            // When the player is in range but cannot be seen
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(parent.MyNavMeshAgent.transform.position, new Vector3(1, 1, 1));
    }
    
}