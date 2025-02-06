using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImprovedFireball : Talent
{
    public override bool Click()
    {
        if (base.Click())
        {
            // Give the player the talent ability
            SpellBook.MyInstance.GetSpell("Fireball").MyCastTime -= 0.1f;
            //Trying to assign
            //SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab = SpellBook.MyInstance.GetSpell("Fireball").MyAlternateSpellPrefab;
 
            return true;
        }

        return false;
    }

    public override string GetDescription()
    {
        return string.Format("Improved Fireball\n<color=#ffd100>Reduce casting time\nof your fireball by 0.1 sec. </color>");
    }
}
