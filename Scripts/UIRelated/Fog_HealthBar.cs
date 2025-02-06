using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fog_HealthBar : MonoBehaviour
{
    [SerializeField]
    private float xRotate;

    [SerializeField]
    private float yRotate;

    private RawImage FogImage;
    // Start is called before the first frame update
    void Awake()
    {
        FogImage = transform.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rect uvFogImage = FogImage.uvRect;
        uvFogImage.x -= xRotate * Time.deltaTime;
        uvFogImage.y -= yRotate * Time.deltaTime;
        FogImage.uvRect = uvFogImage;

        //uvFogImage.x -= .3f * Time.deltaTime;
        //uvFogImage.y -= .15f * Time.deltaTime;
    }
}
