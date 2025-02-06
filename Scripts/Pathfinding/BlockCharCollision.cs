using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharCollision : MonoBehaviour
{

    public CapsuleCollider charCollider;
    public CapsuleCollider charBlockerCollider;


    void Start()
    {
        Physics.IgnoreCollision(charCollider, charBlockerCollider, true);
    }

}
