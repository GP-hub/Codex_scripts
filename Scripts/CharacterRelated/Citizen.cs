using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : NPC
{
    [SerializeField]
    private Dialog dialog;

    public override void Interact()
    {
        base.Interact();
        DialogWindow.MyInstance.SetDialogue(dialog);
    }
}
