using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Window window;

    private Animator animator;

    [SerializeField]
    private bool IsAnimating;
    public bool IsInteracting { get; set; }
    public Animator MyAnimator { get => animator; set => animator = value; }

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        if (IsAnimating)
        {
            InvokeRepeating("RandomIdle", 2f, Random.Range(10, 20));            
        }        
    }

    void RandomIdle()
    {
        if (IsAnimating)
        {
            int x = Random.Range(1, 4);
            if (x == 1)
            {
                MyAnimator.ResetTrigger("Stretch");
                MyAnimator.SetTrigger("Looking_Around");
            }
            if (x == 2)
            {
                MyAnimator.ResetTrigger("Looking_Around");
                MyAnimator.SetTrigger("Stretch");
            }
            if (x == 3)
            {
                MyAnimator.ResetTrigger("Stretch");
                MyAnimator.ResetTrigger("Looking_Around");
            }
        }
    }

    public virtual void Interact()
    {
        if (!IsInteracting)
        {
            //if (IsInteracting == true)
            //{
            //    window.Close();
            //}           
            IsInteracting = true;
            window.Open(this);
        }
    }

    public virtual void StopInteract()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            window.Close();
        }
    }
}
