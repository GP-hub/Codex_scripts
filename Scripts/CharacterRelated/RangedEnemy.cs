using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private GameObject spellPrefabFromSky;

    [SerializeField]
    private Transform exitPoints;

    [SerializeField]
    private float range;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float accuracy;    


    public void RandomRangedAttack()
    {
        int z = Random.Range(0,2);

        if (z==0)
        {
            Shoot();
        }
        if (z == 1)
        {
            ShootFromSky();
        }
    }
    public void Shoot(/*int exitIndex*/)
    {
        Vector3 currentTargetPosition = MyTarget.transform.position + new Vector3(Random.Range(accuracy * -1, accuracy), 1f, Random.Range(accuracy * -1, accuracy));
        // If trail not working, change emitter velocity to 'transform' instead of 'velocity'
        SpellScript s = Instantiate(spellPrefab, exitPoints.position, Quaternion.identity).GetComponent<SpellScript>();
        s.IniatilizeEnemy(currentTargetPosition, damageMin, damageMax, this, range, projectileSpeed, exitPoints.position);
    }

    public void ShootFromSky(/*int exitIndex*/)
    {
        Vector3 currentTargetPosition = MyTarget.transform.position;
        Vector3 aroundTargetPosition = new Vector3(currentTargetPosition.x + Random.Range(accuracy * -1, accuracy), currentTargetPosition.y, currentTargetPosition.z + Random.Range(accuracy * -1, accuracy));
        // If trail not working, change emitter velocity to 'transform' instead of 'velocity'
        SpellScript s = Instantiate(spellPrefabFromSky, aroundTargetPosition, Quaternion.identity).GetComponent<SpellScript>();
        s.IniatilizeEnemy(aroundTargetPosition, damageMin, damageMax, this, range, 0, aroundTargetPosition);
        Debug.Log(accuracy);
    }
}