using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermafrostTalent : Talent
{
    PermafrostDebuff debuff = new PermafrostDebuff();

    private float speedReduction = 50;

    private float reductionIncrease = 20;

    private string nextRank = string.Empty;


    public void Start()
    {
        debuff.MySpeedReduction = speedReduction;
    }

    public override bool Click()
    {
        if (base.Click())
        {
            debuff.MySpeedReduction = speedReduction;

            if (MyCurrentCount < 3)
            {
                speedReduction += reductionIncrease;
                nextRank = $"<color=#ffffff>\n\nNext rank: </color>\n<color=#ffd100>Your Frostbolt applies a debuff\nto the target that reduces\nthe target movement speed by {debuff.MySpeedReduction + reductionIncrease}%.</color>";
            }
            else
            {
                nextRank = string.Empty;
            }

            SpellBook.MyInstance.GetSpell("Frostbolt").MyDebuff = debuff;
            UIManager.MyInstance.RefreshTooltip(this);
            return true;
        }

        return false;
    }

    public override string GetDescription()
    {
        return $"Permafrost<color=#ffd100>\nYour Frostbolt applies a debuff\nto the target that reduces\nthe target movement speed by {debuff.MySpeedReduction}%.</color>{nextRank}";
    }
}
