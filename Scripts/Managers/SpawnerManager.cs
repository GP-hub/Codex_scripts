using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    private bool bossSpawner = false;
    
    //[SerializeField]
    //private float spawnRadius = 10;

    [SerializeField]
    private int numberOfAgents;

    [SerializeField]
    private GameObject[] enemyPrefab;

    [SerializeField]
    private GameObject[] bossPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //BoxCollider b = this.GetComponent<BoxCollider>();
        if (!bossSpawner)
        {
            for (int i = 0; i < numberOfAgents; i++)
            {
                // Choose a random location within the spawnRadius
                //Vector2 randomLoc2d = Random.insideUnitCircle * spawnRadius;
                //Vector3 randomLoc3d = new Vector3(transform.position.x + randomLoc2d.x, transform.position.y, transform.position.z + randomLoc2d.y);

                Vector3 point = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), 0, Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));

                // Make sure the location is on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(point, out hit, 100, 1))
                {
                    point = hit.position;
                }

                // Instantiate and make the enemy a child of this object
                GameObject o = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], point, transform.rotation);
                o.GetComponent<NavMeshAgent>().avoidancePriority = i + 1;
                o.transform.parent = this.transform;
            }
        }
        if (bossSpawner)
        {
            Vector3 point = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x), 0, Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));

            // Make sure the location is on the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(point, out hit, 100, 1))
            {
                point = hit.position;
            }

            // Instantiate and make the enemy a child of this object
            GameObject b = Instantiate(bossPrefab[Random.Range(0, bossPrefab.Length)], point, transform.rotation);
        }
        
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.green; Gizmos.DrawWireSphere(transform.position, spawnRadius);
        //Gizmos.color = Color.green; Gizmos.DrawWireCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
    }
}