using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

class FollowState : IState
{
    private Enemy parent;

    private Transform closestEnemy;

    public static List<Collider> enemies = new List<Collider>();

    private bool checkingDistance = false;

    public float RadiusAroundTarget = .5f;

    private int nextUpdate = 1;

    float elapsed = 0f;

    public List<Enemy> Units = new List<Enemy>();
    public void Enter(Enemy parent)
    {
        Player.MyInstance.AddAttacker(parent);
        this.parent = parent;
        if (parent.MyNavMeshAgent.enabled)
        {
            parent.MyNavMeshAgent.ResetPath();
        }
        checkingDistance = false;
        //if (parent.MyTarget != null && parent.CanSeePlayer())
        //{
        //    parent.MyNavMeshAgent.SetDestination(parent.MyTarget.transform.position);
        //}
    }

    public void Exit()
    {
        Player.MyInstance.RemoveAttacker(parent);

        if (parent.MyNavMeshAgent.enabled)
        {
            parent.MyNavMeshAgent.ResetPath();
        }
        checkingDistance = false;
    }

    public void Update()
    {
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            EnemiesFollowing();
        }        
    }


    private void MakeAgentCircleTarget()
    {
        //Debug.Log(Player.MyInstance.Attackers.Count);

        for (int i = 0; i < Player.MyInstance.Attackers.Count; i++)
        {
            if (parent.IsAlive)
            {
                Player.MyInstance.Attackers[i].MyNavMeshAgent.SetDestination(new Vector3(parent.MyTarget.transform.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Player.MyInstance.Attackers.Count), parent.MyTarget.transform.position.y,
                    parent.MyTarget.transform.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / Player.MyInstance.Attackers.Count)));
            }
            //Player.MyInstance.Attackers[i].MyNavMeshAgent.SetDestination(parent.MyTarget.transform.position);
        }
    }

    private void FollowGuide(Enemy guide)
    {
        parent.MyNavMeshAgent.SetDestination(guide.transform.position);
        parent.MyNavMeshAgent.stoppingDistance = 2f;
    }

    void EnemiesFollowing()
    {
        if (parent.MyTarget != null && parent.CanSeePlayer())
        {
            //float distance = Vector3.Distance(parent.MyTarget.transform.position, parent.transform.position);
            float distance = Vector3.Distance(parent.transform.position, parent.MyTarget.transform.position);
            //float collisionCheckDistance = 2f;
            //bool aboutToCollide = false;
            Vector3 targetPos = new Vector3(parent.MyTarget.transform.position.x - parent.transform.position.x,
                parent.MyTarget.transform.position.y - parent.transform.position.y + 1.5f, parent.MyTarget.transform.position.z - parent.transform.position.z);

            //int cubeLayerIndex = LayerMask.NameToLayer("enemy");
            int layerMask = (1 << 2);
            layerMask = ~layerMask;

            RaycastHit hit2;
            if (Physics.CapsuleCast(parent.transform.position, parent.transform.position, 1.5f, targetPos, out hit2, distance, layerMask/*, QueryTriggerInteraction.Ignore*/))
            {

                if (hit2.collider.transform.parent != null)
                {
                    //if (hit2.collider.transform.parent.CompareTag("Player"))
                    //{
                    //    Debug.Log("On va collider avec le player");
                    //    MakeAgentCircleTarget();
                    //}
                    if (hit2.collider.transform.parent.CompareTag("enemy"))
                    {
                        Enemy guide = hit2.collider.GetComponentInParent<Enemy>();
                        FollowGuide(guide);
                    }
                    else
                    {
                        MakeAgentCircleTarget();
                        parent.MyNavMeshAgent.stoppingDistance = 1f;
                    }
                }
            }
            //if (Physics.CapsuleCast(parent.transform.position, parent.transform.position, 1.5f, targetPos, out hit2, distance, layerMask))
            //{
            //    Debug.Log(hit2.collider.name);
            //    if (hit2.collider.transform.parent != null)
            //    {
            //        if (hit2.collider.transform.parent.CompareTag("Player"))
            //        {
            //            MakeAgentCircleTarget();
            //        }
            //    }
            //}

            //MakeAgentCircleTarget();

            //RaycastHit hit;
            ////if (parent.GetComponent<Rigidbody>().SweepTest(parent.MyTarget.transform.position - parent.transform.position, out hit, distance))
            //if (parent.GetComponent<Rigidbody>().SweepTest(targetPos, out hit, distance, QueryTriggerInteraction.Ignore))
            //{
            //    Debug.DrawRay(parent.transform.position, targetPos, Color.green);
            //    if (hit.collider.transform.parent != null)
            //    {
            //        //Debug.Log(hit.collider.name);
            //        Debug.Log(distance);

            //        if (hit.collider.CompareTag("Player"))
            //        {
            //            Debug.Log("On va collider avec le player");                        
            //        }
            //        if (hit.collider.transform.parent.CompareTag("Player"))
            //        {
            //            Debug.Log("On va collider avec le player");
            //        }

            //    }

            //    //RaycastHit[] hitList = parent.GetComponent<Rigidbody>().SweepTestAll(targetPos, distance, QueryTriggerInteraction.Ignore);
            //    //Debug.Log(hitList.Length);
            //    ////Debug.Log(parent.MyTarget.name);
            //    //for (int i = 0; i < hitList.Length; i++)
            //    //{
            //    //    Debug.Log("collider name : " + hitList[i].collider.name);
            //    //    Debug.Log("distance to collider : " + hitList[i].distance);
            //    //}


            //        //if (hit.collider.transform.parent != null)
            //        //{

            //        //    Debug.Log(hit.collider.transform.parent.name);

            //        //    //if (hit.collider.transform.parent.CompareTag("Player"))
            //        //    //{
            //        //    //    MakeAgentCircleTarget();

            //        //    //    ////aboutToCollide = true;
            //        //    //    //Debug.Log("About to collide while following player");

            //        //    //}
            //        //    //if (hit.collider.transform.parent.CompareTag("enemy"))
            //        //    //{
            //        //    //    Exit();
            //        //    //}
            //        //}

            //        //distanceToCollision = hit.distance;
            //}

            //if (!aboutToCollide)
            //{
            //    MakeAgentCircleTarget();                
            //}


            // ORIGINAL
            //parent.MyNavMeshAgent.SetDestination(parent.MyTarget.transform.position);

            //LayerMask mask = LayerMask.GetMask("Clickable");
            //Collider[] tmp_aroundPlayer = Physics.OverlapCapsule(parent.MyTarget.transform.position, parent.MyTarget.transform.position, 2, mask);

            //Collider[] tmpBox = Physics.OverlapBox(parent.MyNavMeshAgent.transform.position + (parent.MyTarget.transform.position - parent.MyNavMeshAgent.transform.position) * 0.5f, new Vector3(.4f, .4f, distance * .5f), parent.transform.rotation, mask);

            //// if the distance between the enemy & the player is longer than the attack range
            //if (distance > parent.MyAttackRange)
            //{
            //    // if the enemy speed is not close to null
            //    if (parent.MyNavMeshAgent.velocity.magnitude < 1f && tmpBox.Length > 0)
            //    {
            //        enemies.Clear();
            //        enemies.AddRange(tmpBox);
            //        closestEnemy = parent.GetClosestEnemy(enemies, parent.transform);

            //        if (closestEnemy != null && (closestEnemy.position - parent.transform.position).sqrMagnitude <= parent.MyAttackRange)
            //        {
            //            parent.ChangeState(new WaitState());
            //            //Debug.Log("back to following cause the closest ally is a bit far");
            //        }
            //    }
            //}

            if (distance <= parent.MyAttackRange)
            {
                parent.ChangeState(new AttackState());
                //Debug.Log("ChangeState(new AttackState()");
            }
        }
        if (!parent.InRange)
        {
            //Debug.Log("ChangeState(new IdleState()");
            parent.ChangeState(new IdleState());
        }
        else if (!parent.CanSeePlayer())
        {
            //Debug.Log("!parent.CanSeePlayer()");
            // When the player is in range but cannot be seen
        }
    }
}