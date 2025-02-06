using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedLightningbolt : Talent
{

    private int percent = 25;
    public override bool Click()
    {
        if (base.Click())
        {
            Spell lightningbolt = SpellBook.MyInstance.GetSpell("Lightningbolt");
            // Give the player the talent ability
            lightningbolt.MyDamageMin += (lightningbolt.MyDamageMin / 100) * percent;
            return true;
        }

        return false;
    }

    public override string GetDescription()
    {
        return string.Format($"Improved Lightningbolt\n<color=#ffd100>Increase the damage \nof your Lightningbolt by {percent}%. </color>");
    }
}

