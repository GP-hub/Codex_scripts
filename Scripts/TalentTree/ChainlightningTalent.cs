using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainlightningTalent : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            SpellBook.MyInstance.LearnSpell("ChainLightning");
        }

        return false;
    }

    public override string GetDescription()
    {
        return $"Chain Lightning<color=#ffd100>\nStrike multiple enemies\nwith chain lightning.</color>";
    }
}
