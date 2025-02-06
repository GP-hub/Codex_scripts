using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField]
    private Transform exitPoints;

    [SerializeField]
    private float range;

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float accuracy;

    [SerializeField]
    private AnimatorOverrideController animatorOverrideController;

    [Space(10), SerializeField]
    private Move[] move;

    [Space(10), SerializeField]
    private GameObject portal;

    public void Shoot(/*int exitIndex*/)
    {
        Vector3 currentTargetPosition = MyTarget.transform.position + new Vector3(Random.Range(accuracy * -1, accuracy), 1f, Random.Range(accuracy * -1, accuracy));
        // If trail not working, change emitter velocity to 'transform' instead of 'velocity'
        SpellScript s = Instantiate(move[0].spellPrefab, exitPoints.position, Quaternion.identity).GetComponent<SpellScript>();
        s.IniatilizeEnemy(currentTargetPosition, damageMin, damageMax, this, range, projectileSpeed, exitPoints.position);
    }

    public void ShootFromSky(/*int exitIndex*/)
    {
        Vector3 currentTargetPosition = MyTarget.transform.position;
        Vector3 aroundTargetPosition = new Vector3(currentTargetPosition.x + Random.Range(accuracy * -1, accuracy), currentTargetPosition.y, currentTargetPosition.z + Random.Range(accuracy * -1, accuracy));
        // If trail not working, change emitter velocity to 'transform' instead of 'velocity'
        SpellScript s = Instantiate(move[1].spellPrefab, aroundTargetPosition, Quaternion.identity).GetComponent<SpellScript>();
        s.IniatilizeEnemy(aroundTargetPosition, damageMin, damageMax, this, range, 0, aroundTargetPosition);
    }

    public void NovaAroundBoss(/*int exitIndex*/)
    {
        //Vector3 currentTargetPosition = MyTarget.transform.position;
        //Vector3 aroundTargetPosition = new Vector3(currentTargetPosition.x + Random.Range(accuracy * -1, accuracy), currentTargetPosition.y, currentTargetPosition.z + Random.Range(accuracy * -1, accuracy));
        //// If trail not working, change emitter velocity to 'transform' instead of 'velocity'
        SpellScript s = Instantiate(move[2].spellPrefab, this.transform.position, Quaternion.identity).GetComponent<SpellScript>();
        s.IniatilizeEnemy(this.transform.position, damageMin, damageMax, this, range, 0, MyTarget.transform.position);
    }

    public void MeleeAttack()
    {
        // This has been set by animation event
        if (MyTarget == IsAlive && (Vector3.Distance(this.transform.position, MyTarget.transform.position) <= meleeRange) && IsAlive)
        {
            int damage = Random.Range(damageMin, damageMax);
            MyTarget.TakeDamage(damage, this);
        }
    }
    public bool PickMove()
    {
        if (IsAlive)
        {
            int z = Random.Range(0, 2);

            int bossLifePercent = (int)(MyHealth.MyCurrentValue * 100 / initialHealth);

            int distance = (int)Vector3.Distance(this.transform.position, MyTarget.transform.position);

            if (z == 0)
            {
                MyAttackRange = meleeRange;

                if (MyTarget == IsAlive && distance <= move[3].attackRange)
                {
                    animatorOverrideController["Zombie Attack"] = move[3].alternateMoveAnimation;
                    this.MyAttackAnimation = move[3].alternateMoveAnimation;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (z == 1)
            {
                MyAttackRange = range;

                if (bossLifePercent == 100 && distance <= move[0].attackRange)
                {
                    animatorOverrideController["Zombie Attack"] = move[0].alternateMoveAnimation;
                    this.MyAttackAnimation = move[0].alternateMoveAnimation;
                    return true;
                }
                if (bossLifePercent < 100 && bossLifePercent > 50 && distance <= move[0].attackRange)
                {
                    animatorOverrideController["Zombie Attack"] = move[1].alternateMoveAnimation;
                    this.MyAttackAnimation = move[1].alternateMoveAnimation;
                    return true;
                }
                if (bossLifePercent <= 50 && distance <= move[0].attackRange)
                {
                    animatorOverrideController["Zombie Attack"] = move[2].alternateMoveAnimation;
                    this.MyAttackAnimation = move[2].alternateMoveAnimation;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void PortalSpawn()
    {
        Instantiate(portal, new Vector3(this.transform.position.x, 0f, this.transform.position.z), transform.rotation);
    }
}

[System.Serializable]
public class Move
{
    public string moveName;
    [SerializeField] public GameObject spellPrefab;
    [SerializeField] public AnimationClip alternateMoveAnimation;
    [SerializeField] public int attackRange;

    public Move(GameObject newspellPrefab, AnimationClip newalternateMove)
    {
        spellPrefab = newspellPrefab;
        alternateMoveAnimation = newalternateMove;
    }
}