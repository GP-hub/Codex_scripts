using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFrostbolt : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            // Give the player the talent ability
            SpellBook.MyInstance.GetSpell("Frostbolt").MyRange += 3;
            return true;
        }

        return false;
    }

    public override string GetDescription()
    {
        return string.Format("Improved Frostbolt\n<color=#ffd100>Increase the range \nof your Frostbolt by 1m. </color>");
    }
}

