using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private Vector3 offSetPosition;

    [SerializeField]
    private Vector3 offSetRotation;

    public List<Renderer> listOfWalls1;
    public List<Renderer> listOfWalls2;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        transform.rotation *= Quaternion.Euler(offSetRotation);
    }

    void LateUpdate()
    {
        transform.position = target.position + offSetPosition;

        transform.LookAt(target);
        
        MaterialAlpha();
    }

    void MaterialAlpha()
    {
        RaycastHit[] hits;
        LayerMask mask = LayerMask.GetMask("LineOfSight");
        hits = Physics.RaycastAll(Camera.main.transform.position, target.transform.position - Camera.main.transform.position, Vector3.Distance(transform.position, target.position), mask);
        //Debug.DrawRay(Camera.main.transform.position, target.transform.position - Camera.main.transform.position, Color.red);
 
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();
            Renderer MeshRend = hit.transform.GetComponent<MeshRenderer>();
            if (rend == null)
            {
                //rend = hit.transform.GetChild(0).GetComponent<Renderer>();
                Debug.Log(rend.transform.name);
            }
            
            if (rend && !listOfWalls1.Contains(rend))
            {
                listOfWalls1.Add(rend);
                ////Change the material of all hit colliders
                ////to use a transparent shader.
                //rend.material.shader = Shader.Find("Transparent/Diffuse");
                //Color tempColor = rend.material.color;
                //// If we want to make the object a bit transparent instead of disabling the renderer completly
                ////tempColor.a = 0.2F;
                //rend.material.color = tempColor;
                MeshRend.enabled = false;
            }
            foreach (Renderer renderer in listOfWalls1)
            {
                if (renderer.transform.name == rend.transform.name)
                {
                    //rend.material.shader = Shader.Find("Transparent/Diffuse");
                    //Color tempColor = rend.material.color;
                    //// If we want to make the object a bit transparent instead of disabling the renderer completly
                    ////tempColor.a = 0.2F;
                    //rend.material.color = tempColor;
                    MeshRend.enabled = false;
                }
            }
        }

        if (hits.Length == 0)
        {
            for (int i = 0; i < listOfWalls1.Count; i++)
            {
                Renderer MeshRend = listOfWalls1[i].transform.GetComponent<MeshRenderer>();
                //listOfWalls1[i].material.shader = Shader.Find("Transparent/Diffuse");
                //Color tempColor = listOfWalls1[i].material.color;
                // If we want change the alpha of the objet back to normal
                //tempColor.a = 1F;
                MeshRend.enabled = true;
                //listOfWalls1[i].material.color = tempColor;
                listOfWalls1.Remove(listOfWalls1[i]);
            }            
        }
    }
}
