using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpell : SpellScript
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float spellAoE;

    protected bool hit = false;

    [SerializeField]
    private int NovaVisualSize = 1;

    public float SpellAoE { get => spellAoE; set => spellAoE = value; }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!hit /*&& other.tag == "hitbox" && other.tag != "Player" || other.tag == "obstacle"*//*&& other.transform == MyTarget*/)
        {
            puff.transform.localScale = new Vector3(NovaVisualSize, NovaVisualSize, NovaVisualSize);
            GameObject puffInstance = Instantiate(puff, Source.transform.position, puff.transform.rotation);
            //Debug.Log(Source.transform.name);
            Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);
            Destroy(gameObject);
            hit = true;
            //Debug.Log("original parent: " + other.transform.parent.name);

            // Area size max on where to find targets 
            //Collider[] tmp = Physics.OverlapBox(this.transform.position, new Vector3(5, 5, 5), Quaternion.identity, layerMask);
            //Collider[] tmp = Physics.OverlapSphere(this.transform.position, 5f, layerMask);

            LayerMask mask = new LayerMask();

            //LayerMask mask = LayerMask.GetMask("PlayerLayer");
            //LayerMask mask = LayerMask.GetMask("Hit");

            if (Source.tag == "enemy")
            {
                mask = LayerMask.GetMask("PlayerLayer");
                //mask = LayerMask.GetMask("PlayerLayer");
            }
            if (Source.tag == "Player")
            {
                mask = LayerMask.GetMask("Hit");
                //mask = LayerMask.GetMask("Hit");
            }

            Collider[] tmp = Physics.OverlapCapsule(new Vector3(Source.transform.position.x, Source.transform.position.y + 5f, Source.transform.position.z), Source.transform.position, SpellAoE/*, layerMask*/, mask);
            Debug.Log(tmp.Length);
            foreach (Collider collider in tmp)
            {
                Debug.Log("collider name: " + collider.transform.parent.name);
                Debug.Log("layermask" + mask);
                if (collider.transform.parent != null && Source.tag == "Player")
                {
                    if (/*collider.transform.parent.name != other.transform.parent.name &&*/ collider.transform != transform && collider.tag == "hitbox" && collider.transform.parent != Source.transform)
                    {
                        //targets.Add(collider.transform);
                        if (collider.transform.GetComponentInParent<Character>() != null)
                        {
                            collider.transform.GetComponentInParent<Character>().TakeDamage(damage, Source);
                        }                        
                    }
                }
                if (collider.transform.parent.tag == "Player" && Source.tag != "Player")
                {
                    Debug.Log("hit player");
                    collider.transform.GetComponentInParent<Character>().TakeDamage(damage, Source);
                }
            }
        }
    }
}
