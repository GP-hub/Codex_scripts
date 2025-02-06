using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : SpellScript
{
    //[SerializeField]
    //private LayerMask layerMask;

    private List<Transform> targets = new List<Transform>();

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private GameObject groundWarning;


    protected bool hit = false;


    protected override void OnTriggerEnter(Collider other)
    {
        if (!hit && other.tag == "ground")
        {
            puff.transform.localScale = new Vector3(.5f, .5f, .5f);
            GameObject puffInstance = Instantiate(puff, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), puff.transform.rotation);
            Destroy(puffInstance, gameObject.GetComponent<SpellScript>().AoeDelay);
            Destroy(gameObject, 1f);
            Destroy(groundWarning);

            hit = true;

            // Area size max on where to find targets 
            //Collider[] tmp = Physics.OverlapBox(this.transform.position, new Vector3(5, 5, 5), Quaternion.identity, layerMask);
            //Collider[] tmp = Physics.OverlapSphere(this.transform.position, 5f, layerMask);
            Collider[] tmp = Physics.OverlapCapsule(new Vector3(this.transform.position.x, this.transform.position.y + 5f, this.transform.position.z), this.transform.position, radius);

            foreach (Collider collider in tmp)
            {
                if (collider.tag == "Player")
                {
                    collider.transform.GetComponentInParent<Character>().TakeDamage(damage, Source);
                }
            }
        }
    }
}
