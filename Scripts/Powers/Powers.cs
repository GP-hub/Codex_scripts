using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour
{
    private List<int> powers = new List<int>();

    [SerializeField]
    private GameObject alternateFireball;

    private GameObject previousFireballPrefab;

    [SerializeField]
    private Debuff newDebuff;

    MaledictionDebuff malediction = new MaledictionDebuff();

    public void AddPower(int power)
    {
        powers.Add(power);
        LegendaryPowers();
    }

    public void RemovePower(int power)
    {
        powers.Remove(power);
        LegendaryPowers();
        //Debug.Log("powers removed : " + power);

        //if (powers.Count > 0)
        //{
        //    foreach (int Lpower in powers)
        //    {
        //        Debug.Log("we now have the following powers : " + Lpower);
        //    }
        //}
        //else
        //{
        //    Debug.Log("we dont have any power");
        //} 
    }
    public void LegendaryPowers()
    {
        //if (powers.Contains(0))
        //{
        //    Debug.Log(SpellBook.MyInstance.GetSpell("Fireball").MySpeed);
        //    //SpellBook.MyInstance.GetSpell("Fireball").MySpeed -= 0.1f;
        //}

        if (powers.Count > 0)
        {
            //Debug.Log(SpellBook.MyInstance.GetSpell("Fireball").MySpeed);
            //SpellBook.MyInstance.GetSpell("Fireball").MySpeed = 5;

            previousFireballPrefab = SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab;
            SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab = alternateFireball;

            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = malediction;
            Debug.Log(malediction);
        }
        if (powers.Count <= 0)
        {
            SpellBook.MyInstance.GetSpell("Fireball").MyDebuff = null;

            SpellBook.MyInstance.GetSpell("Fireball").MySpellPrefab = previousFireballPrefab;

            // Test of specific spell value change
            //SpellBook.MyInstance.GetSpell("Nova").MySpellPrefab.GetComponent<NovaSpell>().SpellAoE += 6;


            //Debug.Log(SpellBook.MyInstance.GetSpell("Fireball").MySpeed);
            //SpellBook.MyInstance.GetSpell("Fireball").MySpeed = 20;
        }
    }
}
