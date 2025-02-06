using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate void HealthChanged(float health);
public delegate void CharacterRemoved();

public class Enemy : Character/*, IInteractable*/
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private GameObject trailWeapon;

    private IState currentState;

    public enum SpellElem { Fire, Electricity, Frost, Corrosive};

    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private LayerMask losMask;

    private bool rolling = false;

    private IEnumerator PatrollingRoutine;

    [SerializeField]
    public int completionPoints;

    // This is for testing
    [SerializeField]
    protected int damageMin;

    [SerializeField]
    protected int damageMax;

    [SerializeField]
    private float myAttackRange;

    [SerializeField]
    private float myAttackExtraRange;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private AnimationClip myAttackAnimation;

    [SerializeField]
    private float speedMultiplier;

    [SerializeField]
    private float patrolDistance;

    public float MyAttackTime { get; set; }
    public Vector3 MyStartPosition { get; set; }

    private NavMeshAgent agent;

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }

    [SerializeField]
    private float initialAggroRange;
    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector3.Distance(transform.position, MyTarget.transform.position) < MyAggroRange;
        }
    }

    public float MyAttackRange { get => myAttackRange; set => myAttackRange = value; }
    public AnimationClip MyAttackAnimation { get => myAttackAnimation; set => myAttackAnimation = value; }
    public float MyAttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float MyAttackExtraRange { get => myAttackExtraRange; set => myAttackExtraRange = value; }
    public GameObject MyTrailWeapon { get => trailWeapon; set => trailWeapon = value; }
    public LayerMask LosMask { get => losMask; set => losMask = value; }

    protected void Awake()
    {
        health.Initialize(initialHealth, initialHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initialAggroRange;
        ChangeState(new IdleState());
        //PatrollingRoutine = Patrolling();
    }

    protected override void Update()
    {
        MyAnimator.SetFloat("speedMultiplier", speedMultiplier);
        //Debug.Log(currentState);
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            //J'AI CHANGE CA UN MOMENT POUR LE RECHANGER, JE NE SAIS PLUS POURQUOI ...
            //MyAttackTime += Time.deltaTime;

            currentState.Update();

            if (MyTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new IdleState());
            }
            if (IsMoving && MyTarget != null && (Vector3.Distance(this.transform.position, MyTarget.transform.position) <= myAttackRange))
            {
                Debug.Log("IS NOT MOVING BUT SHOULD BE ATTACKING");

                //Debug.Log(currentState);
                //this.MyNavMeshAgent.avoidancePriority = 1;
                //this.MyNavMeshAgent.updatePosition = false;

                ChangeState(new AttackState());
            }
            
        }
        else if (!IsAlive)
        {
            OnDeath();
        }
        base.Update();
    }

    public Character Select()
    {
        healthGroup.alpha = 1;

        return this;
    }

    public void DeSelect()
    {
        //healthGroup.alpha = 0;

        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    public override void TakeDamage(float damage, Character source)
    {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);
                healthGroup.alpha = 1;

                if (!IsAlive)
                {
                    //Player.MyInstance.MyAttackers.Remove(this);
                    source.RemoveAttacker(this);
                    Player.MyInstance.GainXP(XPManager.CalculateXP(this as Enemy));
                    GameManager.MyInstance.GainCompletion(this);
                }
            }
    }

    public void DoDamage()
    {
        // This has been set by animation event
        if (MyTarget == IsAlive && (Vector3.Distance(this.transform.position, MyTarget.transform.position) <= myAttackRange))
        {
            int damage = Random.Range(damageMin, damageMax);
            MyTarget.TakeDamage(damage, this);
        }
        else
        {
            //Debug.Log("Target dead or too far");
        }
    }

    public Transform GetClosestEnemy(List<Collider> enemies, Transform fromThis)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Collider potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.GetComponentInParent<Transform>().position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && dSqrToTarget > 0)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.GetComponentInParent<Transform>();
            }
        }
        return bestTarget;
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();            
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Character target)
    {     
        if (MyTarget == null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            MyAggroRange = initialAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
            target.AddAttacker(this);
        }
    }

    public void Patrol(bool CR_running)
    {
        if (CR_running == true)
        {
            StartCoroutine("Patrolling");
        }
        if (CR_running == false)
        {
            StopCoroutine("Patrolling");
        }
    }

    public IEnumerator Patrolling()
    {
        int delay = Random.Range(4, 8);

        yield return new WaitForSeconds(delay / 2);

        agent = GetComponent<NavMeshAgent>();

        if (false || !IsAlive)
        {
            Debug.Log("check CR_running/break");
            CurrentSpeed = agent.speed;
            yield break;
        }

        while (true && IsAlive)
        {
            //agent.speed = agent.speed * 0.5f;
            //agent.speed = 1f;
            //this.Speed = 1f;
            //agent.speed = CurrentSpeed = 1f;

            //Debug.Log(agent.speed);

            //Vector3 originalPosition = this.transform.position;
            //Vector3 nextMove = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

            if (false || !IsAlive)
            {
                Debug.Log("check CR_running/break");
                CurrentSpeed = agent.speed;
                yield break;
            }

            //Checking if we can see where to patrol next.
            Vector3 targetPatrollingDirection = this.transform.position + Random.insideUnitSphere * patrolDistance;

            RaycastHit los;

            if (Physics.Raycast(transform.position, targetPatrollingDirection, out los, Vector3.Distance(transform.position, targetPatrollingDirection), LosMask))
            {
                //If the path between the enemy and the next patrol point is obstructed, we start the loop from the beginning
                continue;
            }

            yield return new WaitForSeconds(delay);

            if (IsAlive)
            {
                if (agent.enabled && true)
                {
                    //Debug.Log("move to random direction");
                    agent.SetDestination(targetPatrollingDirection);
                }
            }
        }
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            //lootTable.ShowLoot();   
            List<Drop> drops = new List<Drop>();
            // REMEMBER LIST IS EMPTY IF TOO FAR FROM ENEMY COLLIDER
            foreach (IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                if (interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.MyInstance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnDeath()
    {
        if (rolling == false)
        {
            // LOOT DROP TEST
            lootTable.GetLoot();
            rolling = true;
        }
    }
    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(gameObject);
    }

    public bool CanSeePlayer()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit los;

            if (Physics.Raycast(transform.position, targetDirection, out los, Vector3.Distance(transform.position, MyTarget.transform.position), LosMask))
            {
                return false;
            }
        }

        Debug.DrawRay(transform.position, (MyTarget.transform.position - transform.position), Color.red);


        return true;
    }
}
