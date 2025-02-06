using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    // ref to a slot
    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    private GameObject itemGameObject;

    //[SerializeField]
    //private float cooldown;

    //private bool onCooldown = false;

    [SerializeField]
    private int price;

    private Color rarityColor;
    public Sprite MyIcon { get => icon;}
    public int MyStackSize { get => stackSize;}
    public SlotScript MySlot { get => slot; set => slot = value; }
    public Quality MyQuality { get => quality; }
    public string MyTitle { get => title; }
    //public float MyCooldown { get => cooldown; set => cooldown = value; }
    //public bool OnCooldown { get => onCooldown; set => onCooldown = value; }

    public CharButton MyCharButton
    {
        get
        {
            return charButton;
        }

        set
        {            
            MySlot = null;
            charButton = value;
        }
    }

    public int MyPrice { get => price; }
    public GameObject MyItemGameObject { get => itemGameObject; set => itemGameObject = value; }

    //public Button MyItemButton { get => itemButton; set => itemButton = value; }

    //public Text MyItemButtontext { get => itemButtontext; set => itemButtontext = value; }

    public virtual string GetDescription()
    {
        return string.Format("<color={0}><b>{1}</b></color>", QualityColor.MyColors[MyQuality], MyTitle);
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
