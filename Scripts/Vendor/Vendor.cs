using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Vendor : NPC, IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    public VendorItem[] MyItems { get => items; }
}
