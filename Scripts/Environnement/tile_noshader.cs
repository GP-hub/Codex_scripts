using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class tile_noshader : MonoBehaviour
{
    [SerializeField]
    private Material noShaderMaterial;

    public List<GameObject> tiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "ground")
                if (child.GetComponentInChildren<MeshRenderer>().material != null)
                {
                    child.GetComponentInChildren<MeshRenderer>().material = noShaderMaterial;
                    Debug.Log("material replaced");
                }
        }

        //foreach (GameObject tile in gameObject.transform.Find("ground"))
        //{
        //    tiles.Add(tile);
        //    Debug.Log("this is a tile");
        //}
        //Debug.Log("tiles list nbr of entries : " + tiles.Count);
        //for (int i = 0; i < tiles.Count; i++)
        //{
        //    if (tiles[i].GetComponentInChildren<MeshRenderer>().material != null)
        //    {
        //        tiles[i].GetComponentInChildren<MeshRenderer>().material = noShaderMaterial;
        //        Debug.Log("material replaced");
        //    }
        //}
    }

}
