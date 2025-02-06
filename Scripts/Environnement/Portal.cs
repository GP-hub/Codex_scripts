using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isOpen;

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            isOpen = true;
            //closedChest.SetActive(false);
            //openChest.SetActive(true);
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            isOpen = false;
            //closedChest.SetActive(true);
            //openChest.SetActive(false);
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
