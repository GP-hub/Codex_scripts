using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : SpellScript
{
    [SerializeField]
    private LayerMask layerMask;

    //[SerializeField]
    //private LineRenderer lineRenderer;

    private List<Transform> targets = new List<Transform>();

    protected bool hit = false;

    private int targetIndex;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            GameObject puffInstance = Instantiate(puff, this.transform.position, Quaternion.identity);
            Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);
            Destroy(gameObject);

            //lineRenderer = Instantiate(lineRenderer, new Vector3(0, 0, 0), Quaternion.identity);

            //lineRenderer.SetPosition(0, Source.transform.position);
            //Debug.Log("pos0:" + Source.transform.position);

            //lineRenderer.SetPosition(1, this.transform.position);
            //Debug.Log("pos1:" + this.transform.position);

        }
        if (!hit && other.tag == "hitbox" && other.tag != "Player"/*&& other.transform == MyTarget*/)
        {
            hit = true;
            //Debug.Log("original parent: " + other.transform.parent.name);

            // Area size max on where to find targets 
            Collider[] tmp = Physics.OverlapBox(other.transform.position, new Vector3(5, 5, 5), Quaternion.identity, layerMask);
            SphereCollider myCollider = transform.GetComponent<SphereCollider>();
            myCollider.radius = .1f;
            
            //Collider[] tmp = Physics.OverlapCapsule(other.transform.position, other.transform.position, 10, layerMask);

            foreach (Collider collider in tmp)
            {
                if (collider.transform.parent != null)
                {
                    if (collider.transform.parent.name != other.transform.parent.name && collider.transform != transform && collider.tag == "hitbox" && collider.transform.parent != Source.transform)
                    {
                        targets.Add(collider.transform);
                        //Debug.Log(targets.Count);
                        //Debug.Log("targets : " + collider.transform.parent.name);
                    }
                }
            }

            // If needed we can accelerate or modify projectile
            speed *= 1.25f;
            //lifetime *= 2;

            if (targets.Count > 0)
            {
                PickTarget(other);
            }
            else
            {
                PickSingleTarget(other);
            }
        }
    }

    private void Update()
    {
        float distance = 0;
        
        if (MyTarget != null)
        {
            distance = Vector2.Distance(transform.position, MyTarget.transform.position);            
            //Debug.Log("distance : " + distance);
        }

        if (distance < 1.03f) /*(distance < 1.011f)*/
        {
            if (hit && targetIndex < targets.Count)
            {
                Debug.Log(MyTarget.parent.name);
                PickTarget(MyTarget.GetComponent<Collider>());
            }
            else if (MyTarget != null)
            {
                base.OnTriggerEnter(MyTarget.GetComponent<Collider>());
            }
        }
    }

    private void PickTarget(Collider collider)
    {
        Character c = collider.GetComponentInParent<Character>();
        c.TakeDamage(damage, Source);

        GameObject puffInstance = Instantiate(puff, transform.position, Quaternion.identity);
        Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);

        MyTarget = targets[targetIndex];
        targetIndex++;
    }

    private void PickSingleTarget(Collider collider)
    {
        Character c = collider.GetComponentInParent<Character>();
        c.TakeDamage(damage, Source);

        Debug.Log(gameObject.name);
        Destroy(gameObject);
        
        GameObject puffInstance = Instantiate(puff, transform.position, transform.rotation);
        Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);
    }
}