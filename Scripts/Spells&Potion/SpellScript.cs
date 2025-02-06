using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody myRigidbody;

    [SerializeField]
    public GameObject puff;

    protected float speed;

    private float range;

    protected float lifetime;

    private Vector3 enemyExitPoint;

    [SerializeField]
    public float AoeDelay;

    private Transform target;

    private GameObject enemySpellGO;

    public Vector3 MyTargetPos { get; /*private*/ set; }

    private SpellBook spellBook;

    public Transform MyTarget { get; protected set; }

    public Character Source { get; set; }

    protected float damage;

    private Debuff debuff;

    private Spell spellElem;

    // Start is called before the first frame update
    void Start()
    {
        spellBook = GetComponent<SpellBook>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Iniatilize(Vector3 targetPos, float damageMin, float damageMax, float speed, float lifetime, Character source, Debuff debuff, Spell spellElem)
    {  
        this.MyTargetPos = targetPos;
        this.damage = DamageCalculation(spellElem, damageMin, damageMax);
        //this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        this.Source = source;
        this.debuff = debuff;
        this.spellElem = spellElem;
    }
    public void Initialize(Vector3 targetPos, float damageMin, float damageMax, Character source, Debuff debuff, Spell spellElem)
    {
        this.MyTargetPos = targetPos;
        this.damage = DamageCalculation(spellElem, damageMin, damageMax);
        this.Source = source;
        this.debuff = debuff;
    }

    public void Iniatilize(Vector3 targetPos, float damageMin, float damageMax, Character source, float range, float speed, Vector3 enemyCastDirection, Spell spellElem)
    {
        this.MyTargetPos = targetPos;
        this.damage = DamageCalculation(spellElem, damageMin, damageMax);
        this.Source = source;
        this.range = range;
        this.speed = speed;
        this.enemyExitPoint = enemyCastDirection;
    }


    public void Iniatilize(Transform target, float damageMin, float damageMax, Character source, Debuff debuff, Spell spellElem)
    {
        this.MyTarget = target;
        this.damage = DamageCalculation(spellElem, damageMin, damageMax);
        this.Source = source;
        this.debuff = debuff;
    }

    public void Iniatilize(Transform target, float damageMin, float damageMax, Character source, Spell spellElem)
    {
        this.MyTarget = target;
        this.damage = DamageCalculation(spellElem, damageMin, damageMax);
        this.Source = source;
    }

    public void IniatilizeEnemy(Vector3 targetPos, float damageMin, float damageMax, Character source, float range, float speed, Vector3 enemyCastDirection)
    {
        this.MyTargetPos = targetPos;
        this.damage = Mathf.Ceil(Random.Range(damageMin, damageMax));
        this.Source = source;
        this.range = range;
        this.speed = speed;
        this.enemyExitPoint = enemyCastDirection;
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            //Spell newSpellCast = spellBook.CastSpell(spellIndex);
            Vector3 direction = MyTarget.position - transform.position + new Vector3(0,1f,0);
            myRigidbody.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {          
            if (Source.tag != "Player")
            {
                Vector3 normalizeDirection = (MyTargetPos - enemyExitPoint).normalized;
                // If trail not working, change emitter velocity to 'transform' instead of 'velocity'
                gameObject.transform.position += normalizeDirection * speed * Time.deltaTime;
                // was added to turn the arrow towards the target

                if (normalizeDirection != new Vector3(0,0,0))
                {
                    transform.rotation = Quaternion.LookRotation(normalizeDirection);
                }                

                Debug.DrawRay(enemyExitPoint, normalizeDirection, Color.red);
                //StartCoroutine(spellTrajectory(myRigidbody, source.transform.position, this.transform.position, range, enemySpellGO));
                if (Vector3.Distance(enemyExitPoint, this.transform.position) > range)
                {
                    DestroySpell();
                }
            }
            if (Source.tag == "Player")
            {
                myRigidbody.velocity = MyTargetPos.normalized * speed;
                StartCoroutine(DestroyAfterTime(lifetime));
            }            

            //StartCoroutine(ExecuteAfterTime(1));

            //if (transform.position == MyTargetPos)
            //{
            //    DestroySpell();
            //}
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hitbox"/*&& other.transform == MyTarget*/)
        {
            //Debug.Log(other.name);

            if (Source.tag != other.transform.parent.tag/*Source.tag != "Player" && other.transform.parent != null && other.transform.parent.tag == "Player"*/)
            {
                Character c = other.GetComponentInParent<Character>();
                speed = 0;

                c.TakeDamage(damage, Source);

                if (debuff != null)
                {
                    Debuff clone = debuff.Clone();

                    clone.Apply(c);
                }
                //GetComponent<Animator>().SetTrigger("impact");
                myRigidbody.velocity = Vector3.zero;
                MyTarget = null;
                //PuffDestroy.SetActive(true);
                //this.gameObject.SetActive(false);
                Destroy(gameObject);
                if (puff != null) 
                {
                    GameObject puffInstance = Instantiate(puff, transform.position, puff.transform.rotation);
                    Destroy(puffInstance, AoeDelay);
                }                

                // IF WANT TO BE DESTROYED BY PARTICLE DURATION
                //var puffChild = puff.transform.GetChild(0).GetComponent<ParticleSystem>();
                //Destroy(puffInstance, puffChild.main.duration);
            }
        }
        if (other.tag == "obstacle" || other.tag == "ground")
        {
            DestroySpell();
        }
    }

    private void DestroySpell()
    {
        //GetComponent<Animator>().SetTrigger("impact");
        myRigidbody.velocity = Vector3.zero;
        Destroy(gameObject);

        if (puff != null) 
        {
            GameObject puffInstance = Instantiate(puff, transform.position, puff.transform.rotation);
            Destroy(puffInstance, AoeDelay);
        }        
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        DestroySpell();
    }

    private float DamageCalculation(Spell spellElem, float damageMin, float damageMax)
    {
        switch (spellElem.MySpellElem.ToString())
        {
            case "Fire":
                // We are rounding the damage up instead of 'RoundToInt' to avoid problem with a potentiel 0 or null math problem.
                damage = Mathf.Ceil(Random.Range(damageMin * (100 + Player.MyInstance.MyFireStat.MyCurrentValue) / 100 + Player.MyInstance.MyFireDamage.MyCurrentValue, damageMax * (100 + Player.MyInstance.MyFireStat.MyCurrentValue) / 100 + Player.MyInstance.MyFireDamage.MyCurrentValue));
                break;
            case "Frost":
                damage = Mathf.Ceil(Random.Range(damageMin * (100 + Player.MyInstance.MyFrostStat.MyCurrentValue) / 100 + Player.MyInstance.MyFrostDamage.MyCurrentValue, damageMax * (100 + Player.MyInstance.MyFrostStat.MyCurrentValue) / 100 + Player.MyInstance.MyFrostDamage.MyCurrentValue));
                break;
            case "Lightning":
                damage = Mathf.Ceil(Random.Range(damageMin * (100 + Player.MyInstance.MyLightningStat.MyCurrentValue) / 100 + Player.MyInstance.MyLightningDamage.MyCurrentValue, damageMax * (100 + Player.MyInstance.MyLightningStat.MyCurrentValue) / 100 + Player.MyInstance.MyLightningDamage.MyCurrentValue));
                break;
            case "Corrosive":
                damage = Mathf.Ceil(Random.Range(damageMin * (100 + Player.MyInstance.MyCorrosiveStat.MyCurrentValue) / 100 + Player.MyInstance.MyCorrosiveDamage.MyCurrentValue, damageMax * (100 + Player.MyInstance.MyCorrosiveStat.MyCurrentValue) / 100 + Player.MyInstance.MyCorrosiveDamage.MyCurrentValue));
                break;
        }
        return damage;
    }

}
