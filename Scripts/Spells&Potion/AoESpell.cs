using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class AoESpell : MonoBehaviour
{
    //[SerializeField]
    //protected ParticleSystem particleSystem;

    //protected MainModule main;

    //[SerializeField]
    //private SpriteRenderer cloudRenderer;

    //[SerializeField]
    //private SpriteRenderer shadowRenderer;

    //[SerializeField]
    //private Color cloudColor;

    //[SerializeField]
    //private Color shadowColor;

    //[SerializeField]
    //protected Color outOfRangeColor;


    protected List<Enemy> enemies = new List<Enemy>();

    public Character Source { get; set; }

    protected float duration;

    protected float damage;

    protected float elapsed;

    protected float tickElapsed;

    protected bool isDot;

    //private void Awake()
    //{
    //    main = particleSystem.main;
    //}

    // Update is called once per frame
    void Update()
    {
        if (isDot)
        {
            elapsed += Time.deltaTime;

            if (elapsed > duration)
            {
                Remove();
            }

            Execute();
        }
        else
        {
            Execute();

            Remove();
        }
        
    }

    //public abstract void Execute();
    public virtual void Execute()
    {

    }

    public void Initialize(float damage, float duration, bool isDot)
    {
        this.damage = damage;
        this.duration = duration;
        this.isDot = isDot;
        enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            //enemies.Add(other.GetComponent<Enemy>());
            Enter(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "enemy")
        {
            //enemies.Remove(other.GetComponent<Enemy>());
            Exit(other.GetComponent<Enemy>());
        }
    }

    public virtual void Enter(Enemy enemy)
    {
        enemies.Add(enemy.GetComponent<Enemy>());
    }

    public virtual void Exit(Enemy enemy)
    {
        enemies.Remove(enemy.GetComponent<Enemy>());
    }

    public virtual void Remove()
    {
        enemies.Clear();
        Destroy(gameObject);
    }

    //public void InRange()
    //{
    //    main.startColor = Color.white;
    //    shadowRenderer.color = shadowColor;
    //    cloudRenderer.color = cloudColor;
    //}

    //public void OutOfRange()
    //{
    //    main.startColor = outOfRangeColor;
    //    shadowRenderer.color = outOfRangeColor;
    //    cloudRenderer.color = outOfRangeColor;
    //}
}
