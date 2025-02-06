using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{

    [SerializeField]
    private GameObject ClickMoveEffect; 

    [SerializeField]
    private LayerMask myLayerMask;

    [SerializeField]
    private string type;

    [SerializeField]
    private int level;

    [SerializeField]
    private float speed;

    private float currentSpeed;

    [SerializeField]
    protected float initialHealth;

    public Character MyTarget { get; set; }
    public Vector3 MyTargetPos { get; set; }

    //public Transform MyTarget { get; set; }

    private List<Debuff> debuffs = new List<Debuff>();

    private List<Debuff> newDebuffs = new List<Debuff>();

    private List<Debuff> expiredDebuffs = new List<Debuff>();

    public List<Character> Attackers { get; set; } = new List<Character>();

    [SerializeField]
    protected Stats health;
    public Stats MyHealth
    {
        get { return health; }
    }

    public Animator MyAnimator { get; set; }

    private Vector3 direction;

    private Player myPlayer;

    private NavMeshAgent myNavMeshAgent;
    private NavMeshObstacle myNavMeshObstacle;

    public bool IsAttacking { get; set; }

    protected Coroutine actionRoutine;

    [SerializeField]
    private Transform hitBox;


    protected Vector3 targetPosition;

    public bool IsMoving
    {
        get
        {
            return MyNavMeshAgent.velocity.sqrMagnitude != 0;
        }
    }

    public bool IsCasting
    {
        get
        {
            return MyAnimator.GetLayerWeight(MyAnimator.GetLayerIndex("Attack")) == 1;
        }
    }

    public Vector3 Direction { get => direction; set => direction = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    public bool IsAlive 
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public string MyType { get => type; }
    public int MyLevel
    { 
        get
        {
            return level;
        }        
        set
        {
            level = value;
        }
    }

    public NavMeshAgent MyNavMeshAgent { get => myNavMeshAgent; set => myNavMeshAgent = value; }
    public NavMeshObstacle MyNavMeshObstacle { get => myNavMeshObstacle; set => myNavMeshObstacle = value; }

    public Transform MyHitBox { get => hitBox; set => hitBox = value; }
    public float Speed { get => speed; /*private*/ set => speed = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {        
        MyAnimator = GetComponent<Animator>();
        myPlayer = GetComponent<Player>();
        MyNavMeshAgent = GetComponent<NavMeshAgent>();
        MyNavMeshObstacle = GetComponent<NavMeshObstacle>();
        currentSpeed = Speed;        
    }

    // Update is called once per frame
    protected virtual void Update()
    {        
        HandleLayers();
        HandleDebuffs();
    }

    private void FixedUpdate()
    {
        MyNavMeshAgent.speed = currentSpeed;
        Move();
    }

    public void SetDestination()
    {
        if (!IsCasting && IsAlive)
        {
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("ground");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                MyNavMeshAgent.isStopped = false;

                if (hit.collider.tag == "ground")
                {
                    // Define the position we clicked
                    targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);

                    // Finding the closest point to the position we clicked that is ON the navmesh
                    NavMeshHit hitG;
                    NavMesh.SamplePosition(targetPosition, out hitG, 100, 1);

                    // We go to that position
                    MyNavMeshAgent.SetDestination(hitG.position);
                    Instantiate(ClickMoveEffect, hitG.position, Quaternion.identity);
                }

                // NEVER true cause we're checking layer "ground" for 'hit'
                if (hit.collider.tag == "interactable")
                {
                    targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);
                    MyNavMeshAgent.SetDestination(targetPosition);
                }
            }
        } 
    }

   
    public void Move()
    {
        if (IsAlive)
        {
            if (this.tag == "Player")
            {
                if (IsMoving && MyNavMeshAgent.isStopped == false)
                {
                    myPlayer.StopAction();
                    MyNavMeshAgent.SetDestination(targetPosition);
                    MyNavMeshAgent.isStopped = false;

                    if (transform.position == targetPosition)
                    {
                        MyNavMeshAgent.isStopped = true;
                        MyNavMeshAgent.ResetPath();
                        //??
                        Player.MyInstance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //this.MyRigidbody.velocity = Vector3.zero;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                //MyRigidbody.velocity = Direction.normalized;
            }
        }
    }

    public virtual void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("Movement");
            }
            else if (IsAttacking)
            {
                ActivateLayer("Attack");
            }
            else
            {
                ActivateLayer("Idle");
            }
        }
        else
        {
            ActivateLayer("Death");
        }  
    }

    private void HandleDebuffs()
    {
        if (expiredDebuffs.Count > 0)
        {
            foreach (Debuff debuff in expiredDebuffs)
            {
                debuffs.Remove(debuff);
            }

            expiredDebuffs.Clear();
        }

        if (debuffs.Count > 0)
        {
            foreach (Debuff debuff in debuffs)
            {
                debuff.Update();
            }
        }

        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }


    }

    public void ApplyDebuff(Debuff debuff)
    {       
        // check if we have a debuff with the same name
        Debuff tmp = debuffs.Find(x => x.Name == debuff.Name);

        // If thats the case
        if (tmp != null)
        {
            // Extending the duration of the already existing debuff, instead of refreshing it ?
            if (tmp.MyDurationLeft < debuff.MyDuration)
            {
                tmp.MyDuration += debuff.MyDuration - tmp.MyDurationLeft;
            }

            //// We remove the old debuff
            //expiredDebuffs.Add(tmp);
            //// Added for the debuff to play its remove function properly ( might cause problem for some debuffs)
            //tmp.Remove();
        }
        else
        {
            // Apply the new debuff
            this.newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        this.expiredDebuffs.Add(debuff);
    }

    // ORIGINAL
    public virtual void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, Mathf.Lerp(MyAnimator.GetLayerWeight(i), 0, .5f));
            //MyAnimator.SetLayerWeight(i, 0);
        }
        //MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), Mathf.Lerp(MyAnimator.GetLayerWeight(MyAnimator.GetLayerIndex(layerName)), 1, .2f));
        //MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), Mathf.Lerp(0, 1, 1f));
    }

    public void StopMovement()
    {
        if (IsMoving == true || MyNavMeshAgent.velocity.sqrMagnitude != 0)
        {
            MyNavMeshAgent.isStopped = true;
        }
    }

    public virtual void TakeDamage(float damage, Character source)
    {
        if (this is Player)
        {
            damage = damage * Player.MyInstance.MyDamageTakenMultiplier / 100;
        }
        if (source is Player)
        {
            damage = damage * Player.MyInstance.MyDamageDoneMultiplier / 100;
        }

        health.MyCurrentValue -= damage;

        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

        if (health.MyCurrentValue <= 0)
        {

            // This is also manipulated in the 'die' animation (for the enemies only)
            //this.GetComponent<Rigidbody>().isKinematic = true;
            //this.GetComponent<Rigidbody>().detectCollisions = false;

            MyNavMeshAgent.isStopped = true;
            MyNavMeshAgent.ResetPath();
            this.MyNavMeshAgent.enabled = false;

            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
        }
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, true);
    }

    public virtual void AddAttacker(Character attacker)
    {
        if (!Attackers.Contains(attacker))
        {
            Attackers.Add(attacker);
        }
    }
    public virtual void RemoveAttacker(Character attacker)
    {
        Attackers.Remove(attacker);
    }
}
