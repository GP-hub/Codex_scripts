using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeleportSpell : AoESpell
{
    [SerializeField]
    public float AoeDelay;

    [SerializeField]
    private GameObject puff;

    protected bool hit = false;

    public override void Execute()
    {
        if (!hit)
        {
            hit = true;

            // Finding closest point on navmesh
            NavMeshHit hitG;
            NavMesh.SamplePosition(this.transform.position, out hitG, 100, 1);

            // Reseting path and teleporting player to navmesh 
            Player.MyInstance.MyNavMeshAgent.ResetPath();
            Player.MyInstance.transform.position = new Vector3(hitG.position.x, Player.MyInstance.transform.position.y, hitG.position.z);

            // Instantiating Spell visual effect on player position arrival
            GameObject puffInstance = Instantiate(puff, Player.MyInstance.transform.position, puff.transform.rotation);
            Destroy(puffInstance, AoeDelay);
        }
    }

}
