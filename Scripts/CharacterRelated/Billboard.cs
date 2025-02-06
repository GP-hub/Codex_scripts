using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    Quaternion originalRotation;

    private void Start()
    {
        originalRotation = Camera.main.transform.rotation * transform.rotation;
    }
    private void LateUpdate()
    {
        transform.rotation = originalRotation;

        // GOOD : slightly more effective ?
        //transform.forward = Camera.main.transform.forward;

        //Vector3 itemNamePos = Camera.main.WorldToScreenPoint(this.transform.position);
    }

}
