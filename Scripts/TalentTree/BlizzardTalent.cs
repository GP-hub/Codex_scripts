using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardTalent : Talent
{
    [SerializeField]
    private float duration;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float slow;

    public void Start()
    {

    }

    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("Blizzard");
        }

        return false;
    }

    public override string GetDescription()
    {
        return $"Blizzard <color=#ffd100>\nCreate a Blizzard at\nthe target location that\ndoes {damage / duration} damage every second\nfor {duration} seconds and slow\nenemies by {slow} %.</color>";
    }
}
