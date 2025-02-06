using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RainOfFire : Talent
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float damage;

    public void Start()
    {

    }

    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("RainOfFire");
        }

        return false;
    }

    public override string GetDescription()
    {
        return $"Rain of fire<color=#ffd100>\nCreate a Rain of fire at\nthe target location that\ndoes {damage/duration} damage every second\nfor {duration} seconds.</color>";
    }
}
