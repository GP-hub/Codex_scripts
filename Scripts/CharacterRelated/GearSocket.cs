using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class GearSocket : MonoBehaviour
{

    public Animator MyAnimator { get; set; }

    protected SpriteRenderer spriteRenderer;

    private AnimatorOverrideController animatorOverrideController;

    private Animator parentAnimator;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);

        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetXAndY(float x, float z)
    {
        MyAnimator.SetFloat("x", x);
        MyAnimator.SetFloat("z", z);
    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        // Name must match the override controller
        animatorOverrideController["attack01"] = animations[0];
        animatorOverrideController["idle"] = animations[1];
        animatorOverrideController["walk"] = animations[2];
    }

    public void Dequip()
    {
        animatorOverrideController["attack01"] = null;
        animatorOverrideController["idle"] = null;
        animatorOverrideController["walk"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;


    }
}
