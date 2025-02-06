using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThundershockTalent : Talent
{
    ThundershockDebuff debuff = new ThundershockDebuff();

    private string nextRank = string.Empty;

    private float procChance;

    private float procIncrease = 5;


    public void Start()
    {
        procChance = 5;
        debuff.ProcChance = procChance;
    }

    public override bool Click()
    {
        if (base.Click())
        {
            debuff.ProcChance = procChance;

            if (MyCurrentCount < 3)
            {
                procChance += procIncrease;
                nextRank = $"<color=#ffffff>\n\nNext rank: </color>\n<color=#ffd100>Your Lightningbolt has a {debuff.ProcChance + procIncrease}% chance to\nstun the target for {debuff.MyDuration} second(s).</color>";
            }
            else
            {
                nextRank = string.Empty;
            }

            SpellBook.MyInstance.GetSpell("Lightningbolt").MyDebuff = debuff;
            UIManager.MyInstance.RefreshTooltip(this);
            return true;
        }

        return false;
    }

    public override string GetDescription()
    {
        return $"Thundershock<color=#ffd100>\nYour Lightningbolt has a {debuff.ProcChance}% chance to\nstun the target for {debuff.MyDuration} second(s).</color>{nextRank}";
    }
}
