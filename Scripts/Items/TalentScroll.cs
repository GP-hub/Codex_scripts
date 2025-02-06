using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Power_scroll", menuName = "Items/Scroll_Power", order = 2)]
public class PowerScroll : Item, IUseable
{
    [SerializeField]
    private int Cooldown;
    public float MyCooldown => Cooldown;

    bool IUseable.OnCooldown { get; set; }

    public void Use()
    {
        // Opening talent window 
        UIManager.MyInstance.OpenClose(UIManager.MyInstance.menus[11]);

        // Rolling talent choice
        GameManager.MyInstance.RandomPower();

        // Consuming the scroll
        Remove();
    }
}
