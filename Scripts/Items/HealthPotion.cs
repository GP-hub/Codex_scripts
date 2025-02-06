using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealthPotion", menuName ="Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private float healthPercent;

    [SerializeField]
    private int Cooldown;
    public float MyCooldown => Cooldown;

    bool IUseable.OnCooldown { get; set; } 

    public void Use()
    {
        if (Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)
        {
            //Remove();
            if ((Player.MyInstance.MyHealth.MyMaxValue - Player.MyInstance.MyHealth.MyCurrentValue) < (Mathf.RoundToInt(((float)Player.MyInstance.MyHealth.MyMaxValue) * (healthPercent / 100))))
            {
                Player.MyInstance.GetHealth(Mathf.RoundToInt((float)Player.MyInstance.MyHealth.MyMaxValue - Player.MyInstance.MyHealth.MyCurrentValue));
            }
            else
            {
                Player.MyInstance.GetHealth(Mathf.RoundToInt(((float)Player.MyInstance.MyHealth.MyMaxValue) * (healthPercent / 100)));
            }                        
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use ({0}s cooldown): Restore {1}% of your maximum health.</color>", Cooldown, healthPercent);
    }
}
