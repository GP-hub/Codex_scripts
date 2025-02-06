using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullTrapSpell : SpellScript
{
    [SerializeField]
    private LayerMask layerMask;

    private List<Transform> targets = new List<Transform>();

    protected bool hit = false;

    private int targetIndex;

    protected override void OnTriggerEnter(Collider other)
    {
        if (!hit && other.tag == "hitbox" && other.tag != "Player" || other.tag == "obstacle"/*&& other.transform == MyTarget*/)
        {
            puff.transform.localScale = new Vector3(4, 4, 4);
            GameObject puffInstance = Instantiate(puff, new Vector3(this.transform.position.x, this.transform.position.y - 1.25f, this.transform.position.z), puff.transform.rotation);
            Debug.Log(Source.transform.name);
            Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);
            Destroy(gameObject);

            hit = true;
            //Debug.Log("original parent: " + other.transform.parent.name);

            // Area size max on where to find targets 
            //Collider[] tmp = Physics.OverlapBox(this.transform.position, new Vector3(5, 5, 5), Quaternion.identity, layerMask);
            //Collider[] tmp = Physics.OverlapSphere(this.transform.position, 5f, layerMask);
            Collider[] tmp = Physics.OverlapCapsule(new Vector3(this.transform.position.x, this.transform.position.y + 5f, this.transform.position.z), this.transform.position, 5f, layerMask);

            foreach (Collider collider in tmp)
            {
                if (collider.transform.parent != null)
                {
                    if (/*collider.transform.parent.name != other.transform.parent.name &&*/ collider.transform != transform && collider.tag == "hitbox" && collider.transform.parent != Source.transform)
                    {
                        //targets.Add(collider.transform);
                        collider.transform.GetComponentInParent<Character>().TakeDamage(damage, Source);
                    }
                }
            }
        }
    }
}
