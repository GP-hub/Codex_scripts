using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    [SerializeField]
    private Enemy parent;

    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.tag == "Player")
        {
            if (parent == null)
            {
                Debug.Log("parent == null");
            }
            
            parent.SetTarget(other.GetComponent<Character>());
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        parent.MyTarget = null;
    //    }
    //}
}
